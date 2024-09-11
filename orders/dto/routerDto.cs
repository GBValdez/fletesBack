using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using fletesProyect.models;

namespace fletesProyect.orders.dto
{
    public class routerDto
    {
        public routerDto(long idStation, List<routeStation> routeStations, List<stationProduct> stationProducts)
        {
            this.idStation = idStation;
            this.routeStations = routeStations;
            this.stationProducts = stationProducts;
        }
        public long idStation { get; }
        public List<routeStation> routeStations { get; }
        public List<stationProduct> stationProducts { get; }
    }
}