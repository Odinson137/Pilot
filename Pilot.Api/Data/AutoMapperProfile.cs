using AutoMapper;
using Pilot.Api.DTO;
using Pilot.Api.Models;

namespace Pilot.Api.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Company, CompanyDto>();
    }   
}