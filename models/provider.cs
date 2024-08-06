using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.utils;

namespace fletesProyect.models
{
    public class Provider : CommonsModel<long>
    {
        public string name { get; set; } = null!;
        public string? description { get; set; }
        public string nit { get; set; } = null!;
        public string address { get; set; } = null!;
        public string tel { get; set; } = null!;
        public string email { get; set; } = null!;
    }
}