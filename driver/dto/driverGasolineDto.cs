using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.models;

namespace fletesProyect.driver.dto
{
    public class driverGasolineDto
    {
        public driverGasolineDto(long id, modelGasoline modelGasoline)
        {
            this.id = id;
            this.modelGasoline = modelGasoline;
        }
        public long id { get; set; }
        public modelGasoline modelGasoline { get; set; }
    }
}