using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.providersModule.dto;

namespace fletesProyect.station
{
    public class stationDto : stationDtoBase
    {
        public providerDto provider { get; set; } = null!;

    }
}