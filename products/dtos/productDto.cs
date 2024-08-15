using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils.catalogue;

namespace fletesProyect.products
{
    public class productDto : productDtoBase
    {
        public long Id { get; set; }
        public Catalogue brandProduct { get; set; } = null!;
        public Catalogue category { get; set; } = null!;

    }
}