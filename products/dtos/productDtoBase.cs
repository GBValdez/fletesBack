using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.products
{
    public class productDtoBase
    {
        public string name { get; set; } = null!;
        public string description { get; set; } = null!;
        public float weight { get; set; }
        public string imgUrl { get; set; } = null!;

    }
}