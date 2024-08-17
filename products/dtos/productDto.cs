using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils.catalogue;
using project.utils.catalogues.dto;

namespace fletesProyect.products
{
    public class productDto : productDtoBase
    {
        public long Id { get; set; }
        public catalogueDto brandProduct { get; set; } = null!;
        public catalogueDto category { get; set; } = null!;

    }
}