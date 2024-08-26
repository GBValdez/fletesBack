using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using Microsoft.AspNetCore.Mvc;
using project.utils.catalogue;
using project.utils.catalogues;
using project.utils.catalogues.dto;

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
        protected override async Task finallyPost(Catalogue entity, catalogueCreationDto dtoCreation, object queryParams)
        {
            modelGasoline modelGasolineNew = new modelGasoline();
            modelGasolineNew.gasolineLtsKm = 0;
            modelGasolineNew.gasolineTypeId = 12;
            modelGasolineNew.modelId = entity.Id;
            context.Add(modelGasolineNew);
            await context.SaveChangesAsync();

        }
    }
}