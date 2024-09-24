using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.Catalogues;
using AvionesBackNet.utils.dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using project.utils.catalogue;
using project.utils.catalogues;
using project.utils.catalogues.dto;
using project.utils.dto;

namespace fletesProyect.catalogues
{
    [ApiController]
    [Route("TGV")]
    public class gasolineTypeController : cataloguesController
    {
        public gasolineTypeController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
            codCatalogue = "TGV";
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult<resPag<catalogueDto>>> get([FromQuery] pagQueryDto data, [FromQuery] catalogueQueryDto queryParams)
        {
            return base.get(data, queryParams);
        }

        protected override async Task<errorMessageDto> validCataloguePost(Catalogue entity, catalogueCreationDto dtoNew, global::System.Object queryParams)
        {
            bool isDouble = double.TryParse(dtoNew.description, out double result);
            if (!isDouble)
                return new errorMessageDto("El campo description debe ser un numero");
            return null;

        }

        protected override async Task<errorMessageDto> validCataloguePut(Catalogue entity, catalogueCreationDto dtoNew, global::System.Object queryParams)
        {
            bool isDouble = double.TryParse(dtoNew.description, out double result);
            if (!isDouble)
                return new errorMessageDto("El campo description debe ser un numero");

            return null;

        }
    }
}