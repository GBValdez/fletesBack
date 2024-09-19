using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fletesProyect.driver.visits.dto.extra
{
    public class foundOrderDemoDto
    {
        public List<visitDto> routes { get; set; } = new List<visitDto>();
        public driverGasolineDemoDto driver { get; set; }
        public double costTotal { get; set; } = 0;
        public double durationTotal { get; set; } = 0;
        public string ultimeCord { get; set; } = null!;
        public string originCoord { get; set; } = null!;
    }
}