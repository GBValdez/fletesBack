using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.orders.dto
{
    public class orderDtoCreation : orderDtoBase
    {
        public List<orderDetaillDtoCreation> orderDetails { get; set; } = new List<orderDetaillDtoCreation>();
    }
}