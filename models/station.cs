using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils;
using project.utils.catalogue;

namespace fletesProyect.models
{
    public class Station : CommonsModel<long>
    {
        public string cord { get; set; } = null!;
        public TimeSpan openingTime { get; set; }
        public TimeSpan closingTime { get; set; }
        public string tel { get; set; } = null!;
        public string email { get; set; } = null!;
        public long providerId { get; set; }
        public Provider provider { get; set; } = null!;
        public long countryId { get; set; } = 25;
        public Catalogue country { get; set; } = null!;

    }
}