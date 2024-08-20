using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.models;
using fletesProyect.products.productsVehicle.dtos;
using fletesProyect.utils.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project.utils;
using project.utils.dto;

namespace fletesProyect.products.productsVehicle
{
    [ApiController]
    [Route("[controller]")]
    public class productVehicleController : controllerCommons<vehicleProduct, prodVehicleDtoCreation, prodVehicleDto, idDto, object, long>
    {
        public productVehicleController(DBProyContext context, IMapper mapper) : base(context, mapper)
        {
        }

        protected override Task<IQueryable<vehicleProduct>> modifyGet(IQueryable<vehicleProduct> query, idDto queryParams)
        {
            query = query.Where(x => x.productId == queryParams.Id).Include(x => x.typeVehicle);
            return base.modifyGet(query, queryParams);
        }

        public override async Task<ActionResult<prodVehicleDto>> post(prodVehicleDtoCreation newRegister, [FromQuery] object queryParams)
        {
            vehicleProduct vehicleProductFind = await context.VehicleProducts
                .Where(x => x.productId == newRegister.productId && x.typeVehicleId == newRegister.typeVehicleId)
                .FirstOrDefaultAsync();

            if (vehicleProductFind != null)
            {
                if (vehicleProductFind.deleteAt == null)
                {
                    return BadRequest(new errorMessageDto("El vehículo ya esta registrado"));
                }
                else
                {
                    vehicleProductFind.deleteAt = null;
                    vehicleProductFind.quantity = newRegister.quantity;
                    await context.SaveChangesAsync();
                    return Ok(mapper.Map<prodVehicleDto>(vehicleProductFind));
                }
            }

            var vehicleProduct = mapper.Map<vehicleProduct>(newRegister);
            await context.AddAsync(vehicleProduct);
            await context.SaveChangesAsync();
            return Ok(mapper.Map<prodVehicleDto>(vehicleProduct));
        }
    }
}