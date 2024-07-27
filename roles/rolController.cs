using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using project.roles.dto;
using project.users;
using project.utils;
using project.utils.dto;

namespace project.roles
{
    [ApiController]
    [Route("rol")]
    // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]

    public class rolController : controllerCommons<rolEntity, rolCreationDto, rolDto, rolQueryDto, object, string>
    {
        private RoleManager<rolEntity> rolManager;
        private UserManager<userEntity> userManager;
        public rolController(RoleManager<rolEntity> rolManager, AvionesContext contex, IMapper mapper, UserManager<userEntity> userManager) : base(contex, mapper)
        {
            this.rolManager = rolManager;
            this.userManager = userManager;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]

        public override Task<ActionResult<resPag<rolDto>>> get([FromQuery] pagQueryDto data, [FromQuery] rolQueryDto queryParams)
        {
            return base.get(data, queryParams);
        }

        public override async Task<ActionResult<rolDto>> post(rolCreationDto newRegister, [FromQuery] object queryParams)
        {
            rolEntity newRol = new rolEntity
            {
                Name = newRegister.name,
            };
            await rolManager.CreateAsync(newRol);
            return Ok();
        }
        public override async Task<ActionResult> put(rolCreationDto entityCurrent, [FromRoute] string id, [FromQuery] object queryCreation)
        {
            return BadRequest(new errorMessageDto("No se puede modificar el rol"));
        }

        protected override async Task<errorMessageDto> validDelete(rolEntity entity)
        {
            IList<userEntity> list = await userManager.GetUsersInRoleAsync(entity.Name);
            if (list.Count > 0)
                return new errorMessageDto("No se puede eliminar el rol porque esta siendo usado por un usuario");
            return null;
        }
    }
}