using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AvionesBackNet.Models;
using AvionesBackNet.utils.dto;
using fletesProyect.googleMaps.dto;
using GoogleApi;
using GoogleApi.Entities.Maps.Common;
using GoogleApi.Entities.Maps.Directions.Request;
using GoogleApi.Entities.Maps.Directions.Response;
using GoogleApi.Entities.Maps.Geocoding.PlusCode.Request;
using GoogleApi.Entities.Search.Common;
using Microsoft.EntityFrameworkCore;
using project.utils.catalogue;
using project.utils.dto;

namespace fletesProyect.googleMaps
{
    public class googleMapsSvc
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly DBProyContext _context;

        public googleMapsSvc(HttpClient httpClient, IConfiguration configuration, DBProyContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }


        public async Task<errClass<long>> validCountry(double lat, double lng)
        {
            // Reemplaza con tu API key de Google
            string apiKey = _configuration["googleMapKey"];
            string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lng}&key={apiKey}";
            errClass<long> resultFinal = new errClass<long>();
            resultFinal.data = -1;
            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                // Deserializamos el JSON a nuestro objeto GeocodingResponse
                geoCodeResDto geocodingResponse = JsonSerializer.Deserialize<geoCodeResDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (geocodingResponse?.Results != null)
                {
                    // Buscar el componente de dirección con el tipo "country"
                    foreach (var result in geocodingResponse.Results)
                    {
                        var countryComponent = result.Address_Components.Find(ac => ac.Types.Contains("country"));
                        if (countryComponent != null)
                        {
                            Catalogue country = await _context.catalogues.Where(c => c.catalogueTypeId == 7 && c.name == countryComponent.Long_Name).FirstOrDefaultAsync();
                            if (country != null)
                            {
                                resultFinal.data = country.Id;
                                return resultFinal;
                            }
                        }

                    }


                    if (resultFinal.data == -1 || geocodingResponse.Results.Count == 0)
                    {
                        resultFinal.error = new errorMessageDto("El país no esta registrado en la base de datos");
                    }

                    return resultFinal;
                }
                else
                {
                    resultFinal.error = new errorMessageDto("No se encontraron resultados");
                    return resultFinal;
                }
            }
            catch (HttpRequestException ex)
            {
                resultFinal.error = new errorMessageDto("Hubo un error con la api : " + ex.Message);
                return resultFinal;
            }
        }
        public async Task<DirectionsResponse> calculateDistance(List<string> wayPoints)
        {
            if (wayPoints.Count < 2)
                throw new Exception("Se necesitan al menos 2 puntos para calcular la distancia");

            string apiKey = _configuration["googleMapKey"];
            string origin = wayPoints[0];
            string ultimeCord = wayPoints[wayPoints.Count - 1];
            wayPoints.RemoveAt(0);
            wayPoints.RemoveAt(wayPoints.Count - 1);

            // Reemplaza con tu API key de Google}
            DirectionsRequest directionsRequest = new DirectionsRequest
            {
                Key = apiKey,
                Origin = getLocation(origin),
                Destination = getLocation(ultimeCord), // Volvemos al origen
                WayPoints = wayPoints.Select(wp => new GoogleApi.Entities.Maps.Directions.Request.WayPoint(getLocation(wp))).ToList(),
                OptimizeWaypoints = false // No optimizamos, solo necesitamos las distancias
            };
            return await GoogleMaps.Directions.QueryAsync(directionsRequest);
        }

        protected LocationEx getLocation(string cord)
        {
            string[] cords = cord.Split(",");
            CoordinateEx coordinateEx = new CoordinateEx(double.Parse(cords[0]), double.Parse(cords[1]));
            return new LocationEx(coordinateEx);
        }

        public static double Harvesine(string pointA, string pointB)
        {
            string[] pointACoords = pointA.Split(",");
            string[] pointBCoords = pointB.Split(",");
            double lat1 = double.Parse(pointACoords[0]);
            double lon1 = double.Parse(pointACoords[1]);
            double lat2 = double.Parse(pointBCoords[0]);
            double lon2 = double.Parse(pointBCoords[1]);
            const double R = 6371e3; // Radio de la Tierra en metros

            // Convertir las coordenadas de grados a radianes
            double lat1Rad = lat1 * Math.PI / 180;
            double lat2Rad = lat2 * Math.PI / 180;
            double deltaLat = (lat2 - lat1) * Math.PI / 180;
            double deltaLon = (lon2 - lon1) * Math.PI / 180;

            // Fórmula de Haversine
            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distancia
            double distance = R * c; // Resultado en metros

            return distance;
        }
    }
}