using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using fletesProyect.providersModule.dto;
using Microsoft.AspNetCore.Mvc;
using project.utils;

namespace fletesProyect.providersModule
{
    [ApiController]
    [Route("[controller]")]
    public class providerController : controllerCommons<Provider, providerDtoBase, providerDto, object, object, long>
    {
        public providerController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}