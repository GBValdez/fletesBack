using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils;

namespace fletesProyect.models
{
    public class Visit : CommonsModel<long>
    {
        public DateTime estimatedDate { get; set; }
        public DateTime realDate { get; set; }
        public DateTime arrivalDate { get; set; }
        public long stationId { get; set; }
        public Station station { get; set; } = null!;
        public long ordenDetailId { get; set; }
        public ordenDetail ordenDetail { get; set; } = null!;
    }
}