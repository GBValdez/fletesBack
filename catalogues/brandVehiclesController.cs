using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using Microsoft.AspNetCore.Mvc;
using project.utils.catalogues;

namespace fletesProyect.catalogues
{
    [ApiController]
    [Route("MDV")]
    public class brandVehiclesController : cataloguesController
    {
        public brandVehiclesController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
            codCatalogue = "MDV";
        }
    }
}