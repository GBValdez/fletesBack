using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.dto;
using fletesProyect.driver;
using fletesProyect.googleMaps;
using fletesProyect.models;
using fletesProyect.orders.dto;
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
        driversHub _driversHub;
        public OrderController(DBProyContext context, IMapper mapper, HttpContextAccessor httpContext, googleMapsSvc googleMapsSvc, driversHub driversHub) : base(context, mapper)
        {
            _accessor = httpContext;
            _googleMapsSvc = googleMapsSvc;
            _driversHub = driversHub;

        }
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

            Dictionary<long, int> productQuantitiesRequired = newRegister.orderDetails.ToDictionary(od => od.productId, od => od.quantity);

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

            List<long> modelGasolines = await context.modelGasolines
                .Where(mg => idTypeVehicle.Contains(mg.typeVehicleId) && mg.maximumWeight > TOTAL_WEIGHT).Select(mg => mg.modelId).Distinct().ToListAsync();
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
                return validZone.error;
            }

            List<Driver> driversAvailable = await context.Drivers
                .Where(d => d.openingTime <= time && d.closingTime >= time && modelGasolines.Contains(d.modelId) && d.countryOptId == validZone.data)
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

            List<routerDto> routers = new List<routerDto>();
            foreach (Station current in stationsAvailable)
            {
                List<routeStation> route = routeStations.Where(rs => rs.stationAId == current.Id && idStations.Contains(rs.stationBId)).ToList();
                List<stationProduct> stationProductMe = stationProducts.Where(sp => sp.stationId == current.Id).ToList();
                routers.Add(new routerDto(current.Id, route, stationProductMe));
            }

            double minCost = 99999999;
            Func<foundOrderDto, routerDto, routerDto, foundOrderDto> found = (foundOrderDto before, routerDto beforeRoute, routerDto currentRouter) =>
            {
                foundOrderDto myFound = mapper.Map<foundOrderDto>(before);
                double distance = beforeRoute.routeStations.Where(rs => rs.stationBId == currentRouter.idStation).Select(rs => rs.distance).FirstOrDefault();
                // myFound.costTotal += distance*before.driver.;
                // if (minCost < myFound.distanceTotal)
                // return null;

                myFound.routes.Add(currentRouter.idStation);
                return myFound;
            };

            foreach (Driver current in driversAvailable)
            {
                string cord = _driversHub.GetDriverPosition(current.Id);
                if (cord != null)
                {

                }
            }


            return null;
        }
    }
}