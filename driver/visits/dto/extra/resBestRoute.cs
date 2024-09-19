using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.station;

namespace fletesProyect.driver.visits.dto.extra
{
    public class resBestRoute
    {
        public List<foundOrderDemoDto> routes { get; set; } = new List<foundOrderDemoDto>();
        public foundOrderDemoDto bestRoute { get; set; }
        public List<stationDto> stations { get; set; } = new List<stationDto>();

    }
}