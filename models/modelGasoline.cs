using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils;
using project.utils.catalogue;

namespace fletesProyect.models
{
    public class modelGasoline : CommonsModel<long>
    {
        public long gasolineTypeId { get; set; }
        public Catalogue gasolineType { get; set; } = null!;
        public long modelId { get; set; }
        public Catalogue model { get; set; } = null!;
        public long typeVehicleId { get; set; }
        public Catalogue typeVehicle { get; set; } = null!;
        //Gasolina por galón
        public float gasolineLtsKm { get; set; }
        public float maximumWeight { get; set; }

    }
}