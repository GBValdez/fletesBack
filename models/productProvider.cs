using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils;

namespace fletesProyect.models
{
    public class productProvider : CommonsModel<long>
    {
        public long productId { get; set; }
        public product product { get; set; } = null!;
        public long providerId { get; set; }
        public Provider provider { get; set; } = null!;
        public double price { get; set; }
    }
}