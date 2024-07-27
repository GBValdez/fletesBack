using AutoMapper;
using project.roles;
using project.roles.dto;
using project.users;
using project.users.dto;
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

            // CreateMap<Catalogo, catalogueDto>();
            // CreateMap<catalogueCreationDto, Catalogo>();

        }

    }
}