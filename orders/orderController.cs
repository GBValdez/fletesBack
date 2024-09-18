using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils;
using AvionesBackNet.utils.dto;
using fletesProyect.driver;
using fletesProyect.driver.dto;
using fletesProyect.googleMaps;
using fletesProyect.models;
using fletesProyect.orders.dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using project.utils;
using project.utils.dto;

namespace fletesProyect.orders
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : controllerCommons<Orden, orderDtoCreation, orderDto, object, object, long>
    {
        googleMapsSvc _googleMapsSvc;
        IHubContext<driversHub> _driversHub;
        public OrderController(DBProyContext context, IMapper mapper, googleMapsSvc googleMapsSvc, IHubContext<driversHub> driversHub) : base(context, mapper)
        {
            _googleMapsSvc = googleMapsSvc;
            _driversHub = driversHub;

        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "userNormal")]
        public async override Task<ActionResult<orderDto>> post(orderDtoCreation newRegister, [FromQuery] object queryParams)
        {
            return BadRequest(new errorMessageDto("No se puede crear una orden de esta forma"));
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "userNormal")]
        [HttpPost("createOrder")]
        public async Task<ActionResult> createOrder([FromBody] orderDtoCreation newRegister)
        {
            Orden entity = mapper.Map<Orden>(newRegister);
            //Asignamos el id del cliente al que pertenece la orden
            string idClient = User.Claims.FirstOrDefault(c => c.Type == "clientId")?.Value;
            if (idClient == null)
            {
                return BadRequest(new errorMessageDto("No se ha encontrado el id del cliente"));
            }
            entity.clientId = long.Parse(idClient);

            //Buscamos a los vehiculos que tenga compatibilidad con los productos de la orden
            List<long> productIds = newRegister.orderDetails.Select(od => od.productId).ToList();

            Dictionary<long, long> productQuantitiesRequired = newRegister.orderDetails.ToDictionary(od => od.productId, od => od.quantity);

            // IQueryable<long> query = context.VehicleProducts
            //     .Where(vp => productIds.Contains(vp.productId)) // Filtrar solo los productos relevantes
            //     .GroupBy(vp => vp.typeVehicleId) // Agrupar por tipo de vehículo
            //     .Where(g => g
            //         .All(vp => vp.quantity >= productQuantitiesRequired[vp.productId]) // Verificar que puede cargar la cantidad requerida de cada producto
            //          && g.Select(vp => vp.productId).Distinct().Count() == productIds.Count // Asegurarse de que cubre todos los productos
            //     )
            //     .Select(g => g.Key); // Seleccionar el ID del tipo de vehículo compatible

            // List<long> idTypeVehicle = await query.ToListAsync();
            // Obtener los vehicleProducts que coinciden con los productos de la orden desde la base de datos

            List<vehicleProduct> vehicleProducts = await context.VehicleProducts
                .Where(vp => productIds.Contains(vp.productId))
                .ToListAsync(); // Ejecuta la consulta en la base de datos y carga los resultados en memoria

            // Agrupar por typeVehicleId y realizar el filtrado en memoria
            List<long> idTypeVehicle = vehicleProducts
                .GroupBy(vp => vp.typeVehicleId) // Agrupar por tipo de vehículo
                .Where(g => g
                    .All(vp => vp.quantity >= productQuantitiesRequired[vp.productId]) // Verificar en memoria si el vehículo puede cargar las cantidades requeridas
                    && g.Select(vp => vp.productId).Distinct().Count() == productIds.Count // Verificar que cubre todos los productos
                )
                .Select(g => g.Key)
                .ToList(); // Obtener la lista de IDs de los tipos de vehículos compatibles

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

            double lat = double.Parse(newRegister.deliveryCoord.Split(',')[0]);
            double lng = double.Parse(newRegister.deliveryCoord.Split(',')[1]);
            errClass<long> validZone = await _googleMapsSvc.validCountry(lat, lng);
            if (validZone.error != null)
            {
                return BadRequest(validZone.error);
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
                return BadRequest(new errorMessageDto("No hay conductores disponibles en la zona"));
            }
            if (stationsAvailable.Count == 0)
            {
                return BadRequest(new errorMessageDto("No hay estaciones disponibles en la zona"));
            }

            List<long> idStations = stationsAvailable.Select(s => s.Id).ToList();
            List<routeStation> routeStations = await context.routeStations
                .Where(rs => idStations.Contains(rs.stationAId))
                .ToListAsync();

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
                foundOrderDto myFound = Utils.CreateDeepCopy<foundOrderDto>(before);
                Visit visitNew = new Visit();
                visitNew.stationId = currentRouter.station.Id;
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
                myFound.routes.Add(visitNew);

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
            int index = -1;
            foreach (driverGasolineDto current in driverList)
            {
                index++;
                string cord = GetDriverPosition(index);
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
                                finalFound.originCoord = cord;
                            }
                        }
                    }

                }
            }

            if (finalFound.ultimeCord == null)
            {
                return BadRequest(new errorMessageDto("No se pudo encontrar una ruta para la orden"));
            }

            //Guardamos la info
            entity.driverId = finalFound.driver.id;
            entity.originCoord = finalFound.originCoord;
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            //Visitas
            List<string> cords = new List<string>();
            cords.Add(entity.originCoord);
            foreach (Visit visit in finalFound.routes)
            {
                visit.orderId = entity.Id;
                foreach (visitProduct visitProduct in visit.visitProducts)
                {
                    long productId = visitProduct.ordenDetailId;
                    ordenDetail ordenDetail = entity.orderDetails.Where(od => od.productId == productId).FirstOrDefault();
                    visitProduct.ordenDetailId = ordenDetail.Id;
                    stationProduct stationProduct = stationProducts.Where(sp => sp.productId == productId && sp.stationId == visit.stationId).FirstOrDefault();
                    stationProduct.stock -= visitProduct.quantity;
                }
                Station station = stationsAvailable.Where(s => s.Id == visit.stationId).FirstOrDefault();
                cords.Add(station.cord);
                await context.AddAsync(visit);
            }
            cords.Add(newRegister.deliveryCoord);
            // _googleMapsSvc.calculateDistance(cords).Result.Routes.First().Legs.First().
            await context.SaveChangesAsync();

            return Ok();
        }
        private double calculateCost(double distance, driverGasolineDto driver)
        {
            double costGalGas = double.Parse(driver.modelGasoline.gasolineType.description);
            double consumeGalKm = driver.modelGasoline.gasolineLtsKm;
            return distance * consumeGalKm * costGalGas;
        }
        private string? GetDriverPosition(long driverId)
        {
            // if (DriverPositions.TryGetValue(driverId, out var position))
            // {
            //     return position;
            // }
            // return null;
            List<string> positions = new List<string>();
            positions.Add("16.906682, -89.940174");
            positions.Add("16.915402, -89.955067");
            positions.Add("16.930593, -89.931335");
            positions.Add("16.919754, -89.926014");
            positions.Add(null);
            return positions[(int)driverId];
        }
    }
}