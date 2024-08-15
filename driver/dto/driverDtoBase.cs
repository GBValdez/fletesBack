using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.driver.dto
{
    public class driverDtoBase
    {
        public string name { get; set; } = null!;
        public string nit { get; set; } = null!;
        public string licensePlate { get; set; } = null!;
        public string tel { get; set; } = null!;
        public string email { get; set; } = null!;
        public string address { get; set; } = null!;
        public TimeSpan openingTime { get; set; }
        public TimeSpan closingTime { get; set; }
        public int stopLimit { get; set; }
        public float maximumWeight { get; set; }


    }
}