using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.dto;
using fletesProyect.googleMaps;
using fletesProyect.models;
using GoogleApi.Entities.Maps.Directions.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;
using project.utils.dto;

namespace fletesProyect.station
{
    [ApiController]
    [Route("[controller]")]
    public class stationController : controllerCommons<Station, stationDtoCreation, stationDto, object, object, long>
    {
        private googleMapsSvc _googleMapsSvc;
        public stationController(DBProyContext context, IMapper mapper, googleMapsSvc googleMapsSvc) : base(context, mapper)
        {
            _googleMapsSvc = googleMapsSvc;
        }
        protected override async Task<errorMessageDto> validPost(Station entity, stationDtoCreation newRegister, object queryParams)
        {
            double lat = double.Parse(newRegister.cord.Split(",")[0]);
            double lng = double.Parse(newRegister.cord.Split(",")[1]);
            errClass<long> errCountry = await _googleMapsSvc.validCountry(lat, lng);
            if (errCountry.error != null)
            {
                return errCountry.error;
            }
            entity.countryId = errCountry.data;
            return null;
        }
        protected override async Task finallyPost(Station entity, stationDtoCreation dtoCreation, object queryParams)
        {
            List<string> wayPoints = new List<string>();
            // Obtén todas las estaciones que están en el mismo país, excepto la estación actual
            List<Station> stationsConnect = await context.stations
                .Where(s => s.Id != entity.Id && s.countryId == entity.countryId && s.deleteAt == null)
                .ToListAsync();

            int MAX_WAY_POINT = 21; // Máximo número de waypoints que Google Maps puede procesar en una solicitud
            wayPoints.Add(entity.cord); // Añade las coordenadas de la estación actual

            // Añade las coordenadas de las estaciones conectadas
            int countWay = 1;
            foreach (Station item in stationsConnect)
            {
                wayPoints.Add(item.cord);
                wayPoints.Add(entity.cord); // Añade de nuevo las coordenadas de la estación actual para el regreso
                countWay += 2;
                if (countWay == 21)
                {
                    countWay = 1;
                    wayPoints.Add(entity.cord);
                }
            }

            // Procesar los waypoints en grupos debido al límite de la API
            if (wayPoints.Count == 1)
                return;

            for (int i = 0; i < wayPoints.Count; i += MAX_WAY_POINT)
            {
                // Obtener un subconjunto de los waypoints actuales
                List<string> wayPointsAux = wayPoints.GetRange(i, Math.Min(MAX_WAY_POINT, wayPoints.Count - i));
                DirectionsResponse response = await _googleMapsSvc.calculateDistance(wayPointsAux);

                // Asegúrate de que la respuesta es válida y contiene rutas
                if (response.Status == GoogleApi.Entities.Common.Enums.Status.Ok && response.Routes.Any())
                {
                    // Obtén la lista de legs (tramos entre estaciones)
                    List<Leg> listLeg = response.Routes.First().Legs.ToList();

                    // Itera sobre los legs para guardar las distancias de ida y vuelta
                    for (int j = 0; j < listLeg.Count(); j += 2)
                    {
                        // Distancia de ida (desde la estación actual a la otra estación)
                        Leg legCurrent = listLeg[j];
                        routeStation route = new routeStation
                        {
                            stationAId = entity.Id, // Estación actual
                            stationBId = stationsConnect[j / 2].Id, // Otra estación
                            distance = legCurrent.Distance.Value, // Distancia de ida
                            duration = legCurrent.Duration.Value / 60   // Duración de ida
                        };
                        Leg legAux = listLeg[j + 1];
                        // Distancia de vuelta (desde la otra estación a la estación actual)
                        routeStation routeReturn = new routeStation
                        {
                            stationAId = stationsConnect[j / 2].Id, // Otra estación
                            stationBId = entity.Id, // Estación actual
                            distance = legAux.Distance.Value, // Distancia de vuelta
                            duration = legAux.Duration.Value / 60   // Duración de vuelta

                        };

                        // Agregar ambas rutas (ida y vuelta) al contexto
                        await context.routeStations.AddAsync(route);
                        await context.routeStations.AddAsync(routeReturn);
                    }

                    // Guarda los cambios después de procesar los legs
                    await context.SaveChangesAsync();
                }
            }
        }

    }
}