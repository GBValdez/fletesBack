using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.driver.dto;
using fletesProyect.models;

namespace fletesProyect.orders.dto
{
    public class foundOrderDto
    {
        public foundOrderDto(List<orderDetaillDtoCreation> orderDetails, driverGasolineDto driver)
        {
            this.orderDetails = orderDetails;
            this.driver = driver;
        }
        public List<Visit> routes { get; set; } = new List<Visit>();
        public List<orderDetaillDtoCreation> orderDetails { get; set; } = new List<orderDetaillDtoCreation>();
        public driverGasolineDto driver { get; set; }
        public double costTotal { get; set; } = 0;
        public double durationTotal { get; set; } = 0;
        public string ultimeCord { get; set; } = null!;
        public string originCoord { get; set; } = null!;

    }
}