using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using fletesProyect.utils.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;

namespace fletesProyect.products
{
    [ApiController]
    [Route("[controller]")]
    public class productController : controllerCommons<product, productDtoCreation, productDto, idDto, object, long>
    {
        public productController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
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