using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils;

namespace fletesProyect.models
{
    public class stationProduct : CommonsModel<long>
    {
        public long stationId { get; set; }
        public Station station { get; set; } = null!;
        public long productId { get; set; }
        public product product { get; set; } = null!;
        public long stock { get; set; }


    }
}