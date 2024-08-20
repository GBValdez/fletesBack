using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using fletesProyect.providersModule.dto;
using fletesProyect.utils.dto;
using Microsoft.AspNetCore.Mvc;
using project.utils;

namespace fletesProyect.providersModule
{
    [ApiController]
    [Route("[controller]")]
    public class providerController : controllerCommons<Provider, providerDtoBase, providerDto, idDto, object, long>
    {
        public providerController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }
        protected override Task<IQueryable<Provider>> modifyGet(IQueryable<Provider> query, idDto queryParams)
        {
            if (queryParams != null && queryParams.Id != null)
            {
                query = query.Where(x => x.Id == queryParams.Id);
            }
            return base.modifyGet(query, queryParams);
        }
    }
}