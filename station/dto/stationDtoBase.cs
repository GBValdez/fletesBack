using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.providersModule.dto;

namespace fletesProyect.station
{
    public class stationDtoBase
    {
        public string cord { get; set; } = null!;
        public TimeSpan openingTime { get; set; }
        public TimeSpan closingTime { get; set; }
        public string tel { get; set; } = null!;
        public string email { get; set; } = null!;

    }
}