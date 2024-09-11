using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.driver.dto;
using project.users.dto;
using project.users.Models;

namespace fletesProyect.orders.dto
{
    public class orderDto : orderDtoBase
    {
        public string originCoord { get; set; } = null!;
        public DateTime? deliveryDate { get; set; }
        public clientDto client { get; set; } = null!;
        public driverDto driver { get; set; } = null!;
        public List<orderDetailDto> orderDetails { get; set; } = new List<orderDetailDto>();

    }
}