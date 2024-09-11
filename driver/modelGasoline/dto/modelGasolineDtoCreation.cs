using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.driver.modelGasolineModule.dto
{
    public class modelGasolineDtoCreation : modelGasolineDtoBase
    {
        public long gasolineTypeId { get; set; }
        public long modelId { get; set; }
        public long typeVehicleId { get; set; }

    }
}