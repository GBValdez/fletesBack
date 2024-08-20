using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.products.productsVehicle.dtos;
using project.utils.catalogues.dto;

namespace fletesProyect.products.productsVehicle
{
    public class prodVehicleDto : prodVehicleDtoBase
    {
        public long Id { get; set; }
        public catalogueDto typeVehicle { get; set; } = null!;
        public productDto product { get; set; } = null!;

    }
}