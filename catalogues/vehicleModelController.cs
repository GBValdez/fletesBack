using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using AvionesBackNet.utils.Catalogues;
using AvionesBackNet.utils.dto;
using fletesProyect.models;
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
    [Route("MODELDV")]
    public class vehicleModelController : cataloguesController
    {
        public vehicleModelController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
            codCatalogue = "MODELDV";
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "ADMINISTRATOR")]
        public override Task<ActionResult<resPag<catalogueDto>>> get([FromQuery] pagQueryDto data, [FromQuery] catalogueQueryDto queryParams)
        {
            return base.get(data, queryParams);
        }

        protected override async Task finallyPost(Catalogue entity, catalogueCreationDto dtoCreation, object queryParams)
        {
            modelGasoline modelGasolineNew = new modelGasoline();
            modelGasolineNew.gasolineLtsKm = 0;
            modelGasolineNew.gasolineTypeId = 12;
            modelGasolineNew.modelId = entity.Id;
            modelGasolineNew.typeVehicleId = 2;
            context.Add(modelGasolineNew);
            await context.SaveChangesAsync();

        }
    }
}