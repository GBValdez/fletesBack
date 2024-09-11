using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using project.utils.dto;

namespace fletesProyect.googleMaps
{
    [ApiController]
    [Route("googleMaps")]
    public class googleMapsController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public googleMapsController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        [HttpGet("get-country")]
        public async Task<IActionResult> GetCountry([FromQuery] double lat, [FromQuery] double lng)
        {
            // Reemplaza con tu API key de Google
            string apiKey = _configuration["googleMapKey"];
            string requestUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={lat},{lng}&key={apiKey}";

            try
            {
                var response = await _httpClient.GetAsync(requestUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return Content(content, "application/json");
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(new errorMessageDto("Hubo un error con la api : " + ex.Message));
            }
        }
    }
}