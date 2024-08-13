using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.Catalogues;
using AvionesBackNet.utils.dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils.catalogue;
using project.utils.catalogues.dto;
using project.utils.dto;

namespace project.utils.catalogues
{
    public class cataloguesController : controllerCommons<Catalogue, catalogueCreationDto, catalogueDto, catalogueQueryDto, object, long>
    {
        protected string codCatalogue { get; set; }
        public cataloguesController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR,ADMINISTRATOR_AIRLINE")]

        public override Task<ActionResult<resPag<catalogueDto>>> get([FromQuery] pagQueryDto data, [FromQuery] catalogueQueryDto queryParams)
        {
            return base.get(data, queryParams);
        }
        protected override async Task modifyPost(Catalogue entity, object queryParams)
        {
            entity.catalogueTypeId = (await getCatalogueType()).Id;
        }

        protected Task<catalogueType> getCatalogueType()
        {
            return context.catalogueTypes.Where(db => db.code == codCatalogue).FirstOrDefaultAsync();
        }

        protected override async Task<IQueryable<Catalogue>> modifyGet(IQueryable<Catalogue> query, catalogueQueryDto queryParams)
        {
            catalogueType catalogueType = await getCatalogueType();
            return query.Where(db => db.catalogueTypeId == catalogueType.Id).Include(db => db.catalogueParent);
        }
    }
}
