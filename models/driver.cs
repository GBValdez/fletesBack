using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvionesBackNet.Models;
using project.users;
using project.utils;
using project.utils.catalogue;

namespace fletesProyect.models
{
    public class Driver : CommonsModel<long>
    {
        public string name { get; set; } = null!;
        public string nit { get; set; } = null!;
        public string licensePlate { get; set; } = null!;
        public string tel { get; set; } = null!;
        public string email { get; set; } = null!;
        public string address { get; set; } = null!;
        public TimeSpan openingTime { get; set; }
        public TimeSpan closingTime { get; set; }
        public int stopLimit { get; set; }
        public string userId { get; set; }
        public userEntity user { get; set; } = null!;
        public long modelId { get; set; }
        public Catalogue model { get; set; } = null!;
        public long countryOptId { get; set; } = 25;
        public Catalogue countryOpt { get; set; } = null!;

    }
}