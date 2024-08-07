﻿using AutoMapper;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Messenger.Models;

namespace Pilot.Messenger.Data;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<BaseModel, BaseDto>();
        CreateMap<BaseDto, BaseModel>();

        Mapping<Message, MessageDto>();
        Mapping<BaseModel, MessageUserDto>();
    }

    private void Mapping<T, TDto>() where T : BaseModel, new() where TDto : BaseDto
    {
        CreateMap<T?, T?>();
        CreateMap<T?, TDto?>();
        CreateMap<TDto?, T?>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));;
        CreateMap<BaseDto?, T?>().ConvertUsing(src => src == null ? null : new T
        {
            Id = src.Id,
            CreateAt = src.CreateAt,
            ChangeAt = src.ChangeAt,
            DeleteAt = src.DeleteAt
        });
    }
}