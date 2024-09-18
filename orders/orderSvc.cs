using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.models;

namespace fletesProyect.orders
{
    public class orderSvc
    {
        public List<Visit> visits { get; set; } = new List<Visit>();
        public List<stationProduct> stationProducts { get; set; } = new List<stationProduct>();
        public List<Station> stations { get; set; } = new List<Station>();
    }
}