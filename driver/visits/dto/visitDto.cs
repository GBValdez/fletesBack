using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.orders.dto;
using fletesProyect.station;

namespace fletesProyect.driver.visits.dto
{
    public class visitDto : visitDtoBase
    {
        public long Id { get; set; }

        public stationDto station { get; set; } = null!;
        public List<visitProductDto> visitProducts { get; set; } = new List<visitProductDto>();
        public orderDto order { get; set; } = null!;


    }
}