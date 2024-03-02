using AutoMapper;
using pilot_api.DTO;
using pilot_api.Models;

namespace pilot_api.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Company, CompanyDto>();
    }   
}