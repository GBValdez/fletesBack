using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.driver.dto;

namespace fletesProyect.orders.dto
{
    public class foundOrderDto
    {
        public List<long> routes { get; set; } = new List<long>();
        public List<orderDetaillDtoCreation> orderDetails { get; set; } = new List<orderDetaillDtoCreation>();
        public driverDto driver { get; set; }
        public double costTotal { get; set; }

    }
}