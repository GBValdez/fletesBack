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
        // Coordenadas de entrega
        public string deliveryCoord { get; set; } = null!;
        // Coordenadas donde se encuentra el driver en ese momento
        public string originCoord { get; set; } = null!;
        // Fecha de entrega
        public DateTime? deliveryDate { get; set; }
        public long clientId { get; set; }
        public Client client { get; set; } = null!;
        public long driverId { get; set; }
        public Driver driver { get; set; } = null!;
        public List<ordenDetail> orderDetails { get; set; } = new List<ordenDetail>();

    }
}