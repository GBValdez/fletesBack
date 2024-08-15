using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;

namespace fletesProyect.products
{
    [ApiController]
    [Route("[controller]")]
    public class productController : controllerCommons<product, productDtoCreation, productDto, object, object, long>
    {
        public productController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }
        protected override async Task<IQueryable<product>> modifyGet(IQueryable<product> query, object queryParams)
        {
            return query.Include(p => p.brandProduct).Include(p => p.category);
        }
    }
}