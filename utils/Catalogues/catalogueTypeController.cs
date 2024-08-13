using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.utils.Catalogues.dto;
using Microsoft.AspNetCore.Mvc;
using project.utils;
using project.utils.catalogue;

namespace fletesProyect.utils.Catalogues
{
    [ApiController]
    [Route("[controller]")]
    public class catalogueTypeController : controllerCommons<catalogueType, catalogueTypeDtoCreation, catalogueTypeDtoCreation, object, object, long>
    {
        public catalogueTypeController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}