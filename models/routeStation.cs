using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using project.utils;

namespace fletesProyect.models
{
    public class routeStation : CommonsModel<long>
    {
        public long stationAId { get; set; }
        public long stationBId { get; set; }

        [ForeignKey("stationAId")]
        public Station stationA { get; set; } = null!;
        [ForeignKey("stationBId")]
        public Station stationB { get; set; } = null!;
        public double distance { get; set; }

    }
}