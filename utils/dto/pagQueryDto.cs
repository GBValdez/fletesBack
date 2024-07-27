using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AvionesBackNet.utils.dto
{
    public class pagQueryDto
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public bool? all { get; set; } = false;

    }
}