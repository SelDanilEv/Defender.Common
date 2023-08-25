using AutoMapper;
using Defender.Common.DTOs;
using Defender.Common.Entities;

namespace Defender.Common.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        this.CreateMap<AccountInfo, AccountDto>();
        this.CreateMap<UserInfo, UserDto>();
    }
}
