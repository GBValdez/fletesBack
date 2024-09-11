using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.products;

namespace fletesProyect.orders.dto
{
    public class orderDetailDto : orderDetailDtoBase
    {
        public productDto product { get; set; } = null!;


    }
}