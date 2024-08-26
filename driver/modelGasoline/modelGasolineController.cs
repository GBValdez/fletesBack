using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.driver.modelGasolineModule.dto;
using fletesProyect.models;
using fletesProyect.utils.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;
using project.utils.dto;

namespace fletesProyect.driver.modelGasolineModule
{
    [ApiController]
    [Route("[controller]")]
    public class modelGasolineController : controllerCommons<modelGasoline, modelGasolineDtoCreation, modelGasolineDto, idDto, object, long>
    {
        public modelGasolineController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<ActionResult<modelGasolineDto>> post(modelGasolineDtoCreation newRegister, [FromQuery] object queryParams)
        {
            modelGasoline modelGasolineFind = await context.modelGasolines
            .Where(x => x.gasolineTypeId == newRegister.gasolineTypeId && x.modelId == newRegister.modelId)
            .FirstOrDefaultAsync();

            if (modelGasolineFind != null)
            {
                if (modelGasolineFind.deleteAt == null)
                {
                    return BadRequest(new errorMessageDto("El gasto de la gasolina ya esta registrado"));
                }
                else
                {
                    modelGasolineFind.deleteAt = null;
                    modelGasolineFind.gasolineLtsKm = newRegister.gasolineLtsKm;
                    await context.SaveChangesAsync();
                    return Ok(mapper.Map<modelGasolineDto>(modelGasolineFind));
                }
            }

            modelGasoline gasolineModelNew = mapper.Map<modelGasoline>(newRegister);
            await context.AddAsync(gasolineModelNew);
            await context.SaveChangesAsync();
            return Ok(mapper.Map<modelGasoline>(gasolineModelNew));

        }

        protected override Task<IQueryable<modelGasoline>> modifyGet(IQueryable<modelGasoline> query, idDto queryParams)
        {
            query = query.Where(x => x.modelId == queryParams.Id)
                .Include(x => x.gasolineType);
            return base.modifyGet(query, queryParams);
        }
    }
}