using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.users;
using fletesProyect.driver.dto;
using fletesProyect.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.users;
using project.users.dto;
using project.utils;
using project.utils.dto;

namespace fletesProyect.driver
{
    [ApiController]
    [Route("[controller]")]
    public class DriverController : controllerCommons<Driver, driveDtoCreation, driverDto, object, object, long>
    {
        protected userSvc userSvc;
        protected UserManager<userEntity> userManager;
        public DriverController(DBProyContext context, IMapper mapper, userSvc userSvc, UserManager<userEntity> userManager) : base(context, mapper)
        {
            this.userSvc = userSvc;
            this.userManager = userManager;
        }
        protected override Task<IQueryable<Driver>> modifyGet(IQueryable<Driver> query, object queryParams)
        {
            query = query.Include(x => x.brand).Include(x => x.model).Include(x => x.user);
            return base.modifyGet(query, queryParams);
        }


        protected override async Task<errorMessageDto> validPost(driveDtoCreation dtoNew, object queryParams)
        {
            userCreationDto userCreation = new userCreationDto
            {
                email = dtoNew.email,
                password = Guid.NewGuid().ToString() + "C0NTR@S3N@",
                userName = dtoNew.userId
            };
            List<string> roles = new List<string> { "EMPLOYEE" };
            errorMessageDto registerResult = await userSvc.register(userCreation, roles);
            if (registerResult != null)
                return registerResult;
            userEntity user = await context.Users.FirstOrDefaultAsync(u => u.UserName == dtoNew.userId);
            dtoNew.userId = user.Id;
            return null;

        }

        protected override Task modifyPut(Driver entity, driveDtoCreation dtoNew, object queryParams)
        {
            dtoNew.userId = entity.userId;
            return base.modifyPut(entity, dtoNew, queryParams);
        }

        protected override async Task<errorMessageDto> validDelete(Driver entity)
        {
            userEntity user = await context.Users.FindAsync(entity.userId);
            if (user != null)
            {
                user.deleteAt = DateTime.Now.ToUniversalTime();
                await context.SaveChangesAsync();
            }
            return null;

        }
    }
}