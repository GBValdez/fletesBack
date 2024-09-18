using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.dto;
using fletesProyect.driver;
using fletesProyect.driver.dto;
using fletesProyect.googleMaps;
using fletesProyect.models;
using fletesProyect.orders.dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;
using project.utils.dto;

namespace fletesProyect.orders
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : controllerCommons<Orden, orderDtoCreation, orderDto, object, object, long>
    {
        HttpContextAccessor _accessor;
        googleMapsSvc _googleMapsSvc;
        orderSvc _orderSvc;
        driversHub _driversHub;
        public OrderController(DBProyContext context, IMapper mapper, HttpContextAccessor httpContext, googleMapsSvc googleMapsSvc, driversHub driversHub, orderSvc orderSvc) : base(context, mapper)
        {
            _accessor = httpContext;
            _googleMapsSvc = googleMapsSvc;
            _driversHub = driversHub;
            _orderSvc = orderSvc;

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "userNormal")]
        public override Task<ActionResult<orderDto>> post(orderDtoCreation newRegister, [FromQuery] object queryParams)
        {
            return base.post(newRegister, queryParams);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult> delete(long id)
        {
            return base.delete(id);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult> put(orderDtoCreation entityCurrent, [FromRoute] long id, [FromQuery] object queryCreation)
        {
            return base.put(entityCurrent, id, queryCreation);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "userNormal,ADMINISTRATOR")]
        public override Task<ActionResult<resPag<orderDto>>> get([FromQuery] pagQueryDto infoQuery, [FromQuery] object queryParams)
        {
            return base.get(infoQuery, queryParams);
        }

        // public Task<ActionResult> createOrder([FromBody] orderDtoCreation newRegister)
        // {
        // }


        protected override async Task<errorMessageDto> validPost(Orden entity, orderDtoCreation newRegister, object queryParams)
        {
            //Asignamos el id del cliente al que pertenece la orden
            string idClient = User.Claims.FirstOrDefault(c => c.Type == "clientId")?.Value;
            if (idClient == null)
            {
                return new errorMessageDto("No se ha encontrado el id del cliente");
            }
            entity.clientId = long.Parse(idClient);

            //Buscamos a los vehiculos que tenga compatibilidad con los productos de la orden
            List<long> productIds = newRegister.orderDetails.Select(od => od.productId).ToList();

            Dictionary<long, long> productQuantitiesRequired = newRegister.orderDetails.ToDictionary(od => od.productId, od => od.quantity);

            IQueryable<long> query = context.VehicleProducts
                .Where(vp => productIds.Contains(vp.productId)) // Filtrar solo los productos relevantes
                .GroupBy(vp => vp.typeVehicleId) // Agrupar por tipo de vehículo
                .Where(g => g
                    .All(vp => vp.quantity >= productQuantitiesRequired[vp.productId]) // Verificar que puede cargar la cantidad requerida de cada producto
                     && g.Select(vp => vp.productId).Distinct().Count() == productIds.Count // Asegurarse de que cubre todos los productos
                )
                .Select(g => g.Key); // Seleccionar el ID del tipo de vehículo compatible

            List<long> idTypeVehicle = await query.ToListAsync();

            List<product> products = await context.Products
                .Where(p => productIds.Contains(p.Id)).ToListAsync();

            double TOTAL_WEIGHT = products.Sum(p => p.weight);

            List<modelGasoline> modelGasolines = await context.modelGasolines
                .Where(mg => idTypeVehicle.Contains(mg.typeVehicleId) && mg.maximumWeight > TOTAL_WEIGHT).Include(md => md.gasolineType).ToListAsync();
            List<long> modelGasolineIds = modelGasolines.Select(mg => mg.modelId).ToList();
            TimeSpan time = DateTime.Now.TimeOfDay;


            //Estaciones disponibles
            List<stationProduct> stationProducts = await context.stationProducts
                .Where(sp => productIds.Contains(sp.productId) && sp.stock > 0).ToListAsync();
            List<long> stationProductsIds = stationProducts.Select(sp => sp.stationId).Distinct().ToList();
            _orderSvc.stationProducts = stationProducts;

            double lat = double.Parse(newRegister.deliveryCoord.Split(',')[0]);
            double lng = double.Parse(newRegister.deliveryCoord.Split(',')[1]);
            errClass<long> validZone = await _googleMapsSvc.validCountry(lat, lng);
            if (validZone.error != null)
            {
                return validZone.error;
            }

            List<long> listOrdersDriverId = await context.Orders
                .Where(o => o.deliveryDate == null && o.deleteAt == null).Select(o => o.driverId).Distinct().ToListAsync();

            List<Driver> driversAvailable = await context.Drivers
                .Where(d => d.openingTime <= time && d.closingTime >= time && modelGasolineIds.Contains(d.modelId) && d.countryOptId == validZone.data && !listOrdersDriverId.Contains(d.Id))
                .ToListAsync();

            List<Station> stationsAvailable = await context.stations
                .Where(s => s.openingTime <= time && s.closingTime >= time && stationProductsIds.Contains(s.Id) && s.countryId == validZone.data)
                .ToListAsync();

            if (driversAvailable.Count == 0)
            {
                return new errorMessageDto("No hay conductores disponibles en la zona");
            }
            if (stationsAvailable.Count == 0)
            {
                return new errorMessageDto("No hay estaciones disponibles en la zona");
            }

            List<long> idStations = stationsAvailable.Select(s => s.Id).ToList();
            List<routeStation> routeStations = await context.routeStations
                .Where(rs => idStations.Contains(rs.stationAId))
                .ToListAsync();
            _orderSvc.stations = stationsAvailable;

            List<routerDto> routers = new List<routerDto>();
            foreach (Station current in stationsAvailable)
            {
                List<routeStation> route = routeStations.Where(rs => rs.stationAId == current.Id && idStations.Contains(rs.stationBId)).ToList();
                List<stationProduct> stationProductMe = stationProducts.Where(sp => sp.stationId == current.Id).ToList();
                routers.Add(new routerDto(current, route, stationProductMe));
            }

            List<driverGasolineDto> driverList = new List<driverGasolineDto>();
            foreach (Driver current in driversAvailable)
            {
                modelGasoline modelGasoline = modelGasolines.Where(mg => mg.modelId == current.modelId).FirstOrDefault();
                driverList.Add(new driverGasolineDto(current.Id, modelGasoline));
            }

            Func<foundOrderDto, routerDto?, routerDto, double, foundOrderDto> found = null;
            found = (foundOrderDto before, routerDto? beforeRoute, routerDto currentRouter, double minCost) =>
            {
                foundOrderDto myFound = mapper.Map<foundOrderDto>(before);
                Visit visitNew = new Visit();
                visitNew.stationId = currentRouter.station.Id;
                myFound.routes.Add(visitNew);
                //Ver consumo de gasolina
                //------------------------------------------------------
                if (beforeRoute != null)
                {
                    routeStation rCurrent = beforeRoute.routeStations.Where(rs => rs.stationBId == currentRouter.station.Id).FirstOrDefault();

                    myFound.costTotal += calculateCost(rCurrent.distance, myFound.driver);
                    myFound.durationTotal += rCurrent.duration;

                }
                DateTime estimatedDate = DateTime.Now.ToUniversalTime().AddMinutes(myFound.durationTotal);
                visitNew.estimatedDate = estimatedDate;

                if (minCost < myFound.costTotal)
                    return null;
                //------------------------------------------------------
                //Verificar productos
                //------------------------------------------------------
                List<stationProduct> stationProducts = currentRouter.stationProducts;
                foreach (orderDetaillDtoCreation currentDetail in myFound.orderDetails)
                {
                    if (currentDetail.quantity == 0)
                        continue;
                    stationProduct stationProduct = stationProducts.Where(sp => sp.productId == currentDetail.productId).FirstOrDefault();
                    if (stationProduct == null)
                        continue;
                    if (stationProduct.stock == 0)
                        continue;
                    visitProduct visitProductNew = new visitProduct();
                    visitProductNew.quantity = Math.Min(stationProduct.stock, currentDetail.quantity);
                    visitProductNew.ordenDetailId = currentDetail.productId;
                    currentDetail.quantity -= visitProductNew.quantity;
                    visitNew.visitProducts.Add(visitProductNew);
                }

                if (myFound.orderDetails.All(od => od.quantity == 0))
                {
                    myFound.ultimeCord = currentRouter.station.cord;
                    return myFound;
                }
                //------------------------------------------------------
                //Buscar en SubRutas si no hemos cumplido con los productos de la orden
                List<long> idsStationRoutes = myFound.routes.Select(r => r.stationId).ToList();
                List<routeStation> routesMissing = currentRouter.routeStations.Where(rs => !idsStationRoutes.Contains(rs.stationBId)).ToList();
                double minCurrent = minCost;
                foundOrderDto minFound = null;
                foreach (routeStation current in routesMissing)
                {
                    routerDto router = routers.Where(r => r.station.Id == current.stationBId).FirstOrDefault();
                    foundOrderDto foundStation = found(myFound, currentRouter, router, minCurrent);
                    if (foundStation != null)
                    {
                        minCurrent = foundStation.costTotal;
                        minFound = foundStation;
                    }
                }
                return minFound;
            };

            foundOrderDto finalFound = new foundOrderDto(newRegister.orderDetails, null);
            finalFound.costTotal = double.MaxValue;
            string cordDriver = null;
            int index = -1;
            foreach (driverGasolineDto current in driverList)
            {
                index++;
                string cord = _driversHub.GetDriverPosition(index);
                if (cord != null)
                {
                    foreach (routerDto currentRI in routers)
                    {
                        foundOrderDto foundOrder = new foundOrderDto(newRegister.orderDetails, current);
                        // Verificar costo de la posicion del conductor a la primera estacion
                        double distanceC = googleMapsSvc.Harvesine(cord, currentRI.station.cord);
                        foundOrder.costTotal = calculateCost(distanceC, current);
                        foundOrder.durationTotal = distanceC / 0.83;
                        foundOrderDto currentFound = found(foundOrder, null, currentRI, double.MaxValue);
                        if (currentFound != null)
                        {
                            if (currentFound.ultimeCord == null)
                            {
                                continue;
                            }
                            //Verificar la distancia de la ultima estacion a la entrega
                            double distanceUltime = googleMapsSvc.Harvesine(currentFound.ultimeCord, newRegister.deliveryCoord);
                            currentFound.costTotal += calculateCost(distanceUltime, current);
                            currentFound.durationTotal += distanceUltime / 0.83;
                            if (currentFound.costTotal < finalFound.costTotal)
                            {
                                finalFound = currentFound;
                                cordDriver = cord;
                            }
                        }
                    }

                }
            }

            if (finalFound.ultimeCord == null)
            {
                return new errorMessageDto("No se pudo encontrar una ruta para la orden");
            }

            //Guardamos la info
            entity.driverId = finalFound.driver.id;
            entity.originCoord = cordDriver;
            _orderSvc.visits = finalFound.routes;
            return null;
        }

        protected override async Task finallyPost(Orden entity, orderDtoCreation dtoCreation, object queryParams)
        {
            List<string> cords = new List<string>();
            cords.Add(entity.originCoord);
            foreach (Visit visit in _orderSvc.visits)
            {
                visit.orderId = entity.Id;
                foreach (visitProduct visitProduct in visit.visitProducts)
                {
                    long productId = visitProduct.ordenDetailId;
                    ordenDetail ordenDetail = entity.orderDetails.Where(od => od.productId == productId).FirstOrDefault();
                    visitProduct.ordenDetailId = ordenDetail.Id;
                    stationProduct stationProduct = _orderSvc.stationProducts.Where(sp => sp.productId == productId && sp.stationId == visit.stationId).FirstOrDefault();
                    stationProduct.stock -= visitProduct.quantity;
                }
                Station station = _orderSvc.stations.Where(s => s.Id == visit.stationId).FirstOrDefault();
                cords.Add(station.cord);
                await context.visits.AddAsync(visit);
            }
            cords.Add(dtoCreation.deliveryCoord);
            // _googleMapsSvc.calculateDistance(cords).Result.Routes.First().Legs.First().
            await context.SaveChangesAsync();

        }
        private double calculateCost(double distance, driverGasolineDto driver)
        {
            double costGalGas = double.Parse(driver.modelGasoline.gasolineType.description);
            double consumeGalKm = driver.modelGasoline.gasolineLtsKm;
            return distance * consumeGalKm * costGalGas;
        }
    }
}