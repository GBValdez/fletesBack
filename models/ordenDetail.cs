using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils;

namespace fletesProyect.models
{
    public class ordenDetail : CommonsModel<long>
    {
        public long orderId { get; set; }
        public Orden order { get; set; } = null!;
        public long productId { get; set; }
        public product product { get; set; } = null!;
        public int quantity { get; set; }
    }
}