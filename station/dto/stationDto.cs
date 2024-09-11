using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.providersModule.dto;
using project.utils.catalogues.dto;

namespace fletesProyect.station
{
    public class stationDto : stationDtoBase
    {
        public long Id { get; set; }
        public providerDto provider { get; set; } = null!;
        public catalogueDto country { get; set; } = null!;

    }
}