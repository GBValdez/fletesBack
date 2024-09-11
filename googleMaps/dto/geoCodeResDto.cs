using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.googleMaps.dto
{
    public class geoCodeResDto
    {
        public List<resultGeoDto> Results { get; set; }
        public string Status { get; set; }
    }
}