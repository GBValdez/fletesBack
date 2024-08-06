using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils;
using project.utils.catalogue;

namespace fletesProyect.models
{
    public class vehicleProduct : CommonsModel<long>
    {
        public long typeVehicleId { get; set; }
        public Catalogue typeVehicle { get; set; } = null!;
        public long productId { get; set; }
        public product product { get; set; } = null!;
    }
}