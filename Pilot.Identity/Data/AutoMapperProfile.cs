using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.Models;
using Pilot.Identity.DTO;
using Pilot.Identity.Models;

namespace Pilot.Identity.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BaseModel, BaseDto>();
        
        CreateMap<UserModel, UserDto>();
        CreateMap<UserDto, UserModel>();
        
        CreateMap<UserModel, UserModel>();
    }   
}