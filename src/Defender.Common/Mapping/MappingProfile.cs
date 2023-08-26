using AutoMapper;
using Defender.Common.DTOs;
using Defender.Common.Entities;

namespace Defender.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AccountInfo, AccountDto>();
        CreateMap<UserInfo, UserDto>();
    }
}
