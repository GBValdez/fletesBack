using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.orders.dto;

namespace fletesProyect.driver.visits.dto
{
    public class visitProductDto : visitProductDtoBase
    {
        public long Id { get; set; }
        public orderDetailDto ordenDetail { get; set; } = null!;
    }
}