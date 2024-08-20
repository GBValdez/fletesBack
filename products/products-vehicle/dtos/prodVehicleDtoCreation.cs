using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.products.productsVehicle.dtos
{
    public class prodVehicleDtoCreation : prodVehicleDtoBase
    {
        public long typeVehicleId { get; set; }
        public long productId { get; set; }

    }
}