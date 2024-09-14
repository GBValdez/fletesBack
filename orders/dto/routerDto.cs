using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.models;

namespace fletesProyect.orders.dto
{
    public class routerDto
    {
        public routerDto(Station Station, List<routeStation> routeStations, List<stationProduct> stationProducts)
        {
            this.station = Station;
            this.routeStations = routeStations;
            this.stationProducts = stationProducts;
        }
        public Station station { get; }
        public List<routeStation> routeStations { get; }
        public List<stationProduct> stationProducts { get; }
    }
}