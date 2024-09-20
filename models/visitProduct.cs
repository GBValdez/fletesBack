using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using project.utils;

namespace fletesProyect.models
{
    public class visitProduct : CommonsModel<long>
    {
        public long ordenDetailId { get; set; }
        public ordenDetail ordenDetail { get; set; } = null!;
        public long quantity { get; set; }
        public long visitId { get; set; }
        public Visit visit { get; set; } = null!;
        [NotMapped]
        public double price { get; set; }
    }
}