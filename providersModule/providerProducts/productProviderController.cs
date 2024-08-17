using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using fletesProyect.providersModule.providerProducts.dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;

namespace fletesProyect.providersModule.providerProducts
{
    [ApiController]
    [Route("[controller]")]
    public class productProviderController : controllerCommons<productProvider, productProviderDtoCreation, productProviderDto, productProviderQueryDto, object, long>
    {
        public productProviderController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override Task<IQueryable<productProvider>> modifyGet(IQueryable<productProvider> query, productProviderQueryDto queryParams)
        {
            query = query.Where(x => x.providerId == queryParams.providerId)
                .Include(x => x.product)
                .ThenInclude(x => x.category)
                .Include(x => x.product)
                .ThenInclude(x => x.brandProduct);

            return base.modifyGet(query, queryParams);
        }
    }
}