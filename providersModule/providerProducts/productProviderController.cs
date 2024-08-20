using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using fletesProyect.providersModule.providerProducts.dtos;
using fletesProyect.utils.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;
using project.utils.dto;

namespace fletesProyect.providersModule.providerProducts
{
    [ApiController]
    [Route("[controller]")]
    public class productProviderController : controllerCommons<productProvider, productProviderDtoCreation, productProviderDto, idDto, object, long>
    {
        public productProviderController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override Task<IQueryable<productProvider>> modifyGet(IQueryable<productProvider> query, idDto queryParams)
        {
            query = query.Where(x => x.providerId == queryParams.Id)
                .Include(x => x.product)
                .ThenInclude(x => x.category)
                .Include(x => x.product)
                .ThenInclude(x => x.brandProduct);

            return base.modifyGet(query, queryParams);
        }
        public override async Task<ActionResult<productProviderDto>> post(productProviderDtoCreation newRegister, [FromQuery] object queryParams)
        {
            productProvider productProviderFind = await context.productProviders
                .Where(x => x.providerId == newRegister.providerId && x.productId == newRegister.productId)
                .FirstOrDefaultAsync();

            if (productProviderFind != null)
            {
                if (productProviderFind.deleteAt == null)
                {
                    return BadRequest(new errorMessageDto("El producto ya esta registrado"));
                }
                else
                {
                    productProviderFind.deleteAt = null;
                    productProviderFind.price = newRegister.price;
                    await context.SaveChangesAsync();
                    return Ok(mapper.Map<productProviderDto>(productProviderFind));
                }
            }

            var productProvider = mapper.Map<productProvider>(newRegister);
            await context.AddAsync(productProvider);
            await context.SaveChangesAsync();
            return Ok(mapper.Map<productProviderDto>(productProvider));
        }
    }
}