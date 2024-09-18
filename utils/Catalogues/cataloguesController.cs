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



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult<catalogueDto>> post(catalogueCreationDto newRegister, [FromQuery] object queryParams)
        {
            return base.post(newRegister, queryParams);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult> put(catalogueCreationDto entityCurrent, [FromRoute] long id, [FromQuery] object queryParams)
        {
            return base.put(entityCurrent, id, queryParams);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult> delete(long id)
        {
            return base.delete(id);
        }


        protected override async Task<errorMessageDto> validPost(Catalogue entity, catalogueCreationDto dtoNew, object queryParams)
        {
            entity.catalogueTypeId = (await getCatalogueType()).Id;
            return null;
        }

        protected Task<catalogueType> getCatalogueType()
        {
            return context.catalogueTypes.Where(db => db.code == codCatalogue).FirstOrDefaultAsync();
        }

        protected override async Task<IQueryable<Catalogue>> modifyGet(IQueryable<Catalogue> query, catalogueQueryDto queryParams)
        {
            catalogueType catalogueType = await getCatalogueType();
            if (queryParams.name != null)
                query = query.Where(db => db.name.Contains(queryParams.name));
            if (queryParams.catalogueParentId != null)
                query = query.Where(db => db.catalogueParentId == queryParams.catalogueParentId);
            if (queryParams.id != null)
                query = query.Where(db => db.Id == queryParams.id);

            return query.Where(db => db.catalogueTypeId == catalogueType.Id).Include(db => db.catalogueParent);
        }
    }
}
