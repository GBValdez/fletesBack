using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvionesBackNet.Models;
using project.users.Models;
using project.utils;

namespace fletesProyect.models
{
    public class Orden : CommonsModel<long>
    {
        public string deliveryCoord { get; set; } = null!;
        public DateTime deliveryDate { get; set; }
        public DateTime orderDate { get; set; }
        public long clientId { get; set; }
        public Client client { get; set; } = null!;
        public long driverId { get; set; }
        public Driver driver { get; set; } = null!;

    }
}