using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.Catalogues;
using AvionesBackNet.utils.dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.utils.catalogues;
using project.utils.catalogues.dto;
using project.utils.dto;

namespace fletesProyect.catalogues
{
    [ApiController]
    [Route("CDP")]
    public class productCategoryController : cataloguesController
    {
        public productCategoryController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
            codCatalogue = "CDP";
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult<resPag<catalogueDto>>> get([FromQuery] pagQueryDto data, [FromQuery] catalogueQueryDto queryParams)
        {
            return base.get(data, queryParams);
        }

    }
}