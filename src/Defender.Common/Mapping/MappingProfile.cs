using AutoMapper;

namespace Defender.Common.Mapping;

public class BaseMappingProfile : Profile
{
    public BaseMappingProfile()
    {
        CreateMap<Clients.UserManagement.UserDto, DTOs.UserDto>();

        CreateMap<Clients.Identity.UserDto, DTOs.UserDto>();
        CreateMap<Clients.Identity.AccountDto, DTOs.AccountDto>();
    }
}
