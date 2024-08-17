using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.driver.dto
{
    public class driveDtoCreation : driverDtoBase
    {
        public string userId { get; set; }
        public long brandId { get; set; }
        public long modelId { get; set; }

    }
}