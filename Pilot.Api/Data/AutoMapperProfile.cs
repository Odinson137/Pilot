using AutoMapper;
using Pilot.Api.DTO;
using Pilot.Contracts.Models;

namespace Pilot.Api.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Company, CompanyDto>();
    }   
}