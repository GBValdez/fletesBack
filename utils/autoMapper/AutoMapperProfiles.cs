using AutoMapper;
using AvionesBackNet.Models;
using fletesProyect.driver.dto;
using fletesProyect.driver.modelGasolineModule.dto;
using fletesProyect.models;
using fletesProyect.orders.dto;
using fletesProyect.products;
using fletesProyect.products.productsVehicle;
using fletesProyect.products.productsVehicle.dtos;
using fletesProyect.providersModule.dto;
using fletesProyect.providersModule.providerProducts.dtos;
using fletesProyect.station;
using fletesProyect.utils.Catalogues.dto;
using project.roles;
using project.roles.dto;
using project.users;
using project.users.dto;
using project.users.Models;
using project.utils.catalogue;
using project.utils.catalogues.dto;

namespace project.utils.autoMapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<userEntity, userDto>()
            .ForMember(userDtoId => userDtoId.isActive, options => options.MapFrom(src => src.deleteAt == null));
            ;

            CreateMap<rolEntity, rolDto>();
            CreateMap<rolCreationDto, rolEntity>();
            CreateMap<Client, clientDto>();
            CreateMap<clientCreationDto, Client>();
            CreateMap<clientCreationDto, userCreationDto>();
            CreateMap<Catalogue, catalogueDto>();
            CreateMap<catalogueCreationDto, Catalogue>();
            CreateMap<catalogueTypeDtoCreation, catalogueType>();
            CreateMap<catalogueType, catalogueTypeDtoCreation>();
            CreateMap<Provider, providerDto>();
            CreateMap<providerDtoBase, Provider>();
            CreateMap<Station, stationDto>();
            CreateMap<stationDtoCreation, Station>();
            CreateMap<product, productDto>();
            CreateMap<productDtoCreation, product>();

            CreateMap<Driver, driverDto>();
            CreateMap<driveDtoCreation, Driver>();

            CreateMap<productProvider, productProviderDto>();
            CreateMap<productProviderDtoCreation, productProvider>();

            CreateMap<vehicleProduct, prodVehicleDto>();
            CreateMap<prodVehicleDtoCreation, vehicleProduct>();

            CreateMap<modelGasoline, modelGasolineDto>();
            CreateMap<modelGasolineDtoCreation, modelGasoline>();

            CreateMap<foundOrderDto, foundOrderDto>();

            CreateMap<Orden, orderDto>();
            CreateMap<orderDtoCreation, Orden>();

            CreateMap<ordenDetail, orderDetailDto>();
            CreateMap<orderDetaillDtoCreation, ordenDetail>();
        }

    }
}