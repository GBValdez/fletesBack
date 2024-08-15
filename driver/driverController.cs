using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.driver.dto;
using fletesProyect.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;

namespace fletesProyect.driver
{
    [ApiController]
    [Route("[controller]")]
    public class DriverController : controllerCommons<Driver, driveDtoCreation, driverDto, object, object, long>
    {
        public DriverController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }
        protected override Task<IQueryable<Driver>> modifyGet(IQueryable<Driver> query, object queryParams)
        {
            query = query.Include(x => x.brand).Include(x => x.model).Include(x => x.model);
            return base.modifyGet(query, queryParams);
        }
    }
}