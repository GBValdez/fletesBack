using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using Microsoft.AspNetCore.Mvc;
using project.utils;

namespace fletesProyect.station
{
    [ApiController]
    [Route("[controller]")]
    public class stationController : controllerCommons<Station, stationDtoCreation, stationDto, object, object, long>
    {
        public stationController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}