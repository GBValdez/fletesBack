using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.driver.visits.dto
{
    public class visitDtoBase
    {
        public DateTime estimatedDate { get; set; }
        public DateTime? realDate { get; set; }
        public DateTime? arrivalDate { get; set; }

    }
}