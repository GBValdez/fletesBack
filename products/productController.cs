using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.dto;
using fletesProyect.models;
using fletesProyect.utils.dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;
using project.utils.dto;

namespace fletesProyect.products
{
    [ApiController]
    [Route("[controller]")]
    public class productController : controllerCommons<product, productDtoCreation, productDto, idDto, object, long>
    {
        public productController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "userNormal,ADMINISTRATOR")]
        public override Task<ActionResult<resPag<productDto>>> get([FromQuery] pagQueryDto infoQuery, [FromQuery] idDto queryParams)
        {
            return base.get(infoQuery, queryParams);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult<productDto>> post(productDtoCreation newRegister, [FromQuery] object queryParams)
        {
            return base.post(newRegister, queryParams);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult> put(productDtoCreation entityCurrent, [FromRoute] long id, [FromQuery] object queryCreation)
        {
            return base.put(entityCurrent, id, queryCreation);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult> delete(long id)
        {
            return base.delete(id);
        }
        protected override async Task<IQueryable<product>> modifyGet(IQueryable<product> query, idDto queryParams)
        {
            if (queryParams != null && queryParams.Id != null)
            {
                query = query.Where(x => x.Id == queryParams.Id);
            }

            return query.Include(p => p.brandProduct).Include(p => p.category);
        }
    }
}