﻿using MediatR;
using Pilot.Contracts.Base;
using Pilot.SqrsController.Interfaces;
using Pilot.SqrsController.Queries;

namespace Pilot.SqrsController.Commands;

public record AddValueCommand<TDto> : BaseQuery, IRequest<TDto>, IBaseCommand where TDto : BaseDto
{
    public AddValueCommand(TDto ValueDto, int UserId)
    {
        this.ValueDto = ValueDto;
        this.UserId = UserId;
        Url = $"api/{GetModelName<TDto>()}";
    }

    public object ValueDto { get; init; }
    public int UserId { get; init; }
    public string Url { get; init; }
}

public record UpdateValueCommand<TDto> : BaseQuery, IRequest<TDto>, IBaseCommand where TDto : BaseDto
{
    public UpdateValueCommand(TDto ValueDto, int UserId)
    {
        this.ValueDto = ValueDto;
        this.UserId = UserId;
        Url = $"api/{GetModelName<TDto>()}";
    }

    public object ValueDto { get; init; }
    public int UserId { get; init; }
    public string Url { get; init; }
}

public record DeleteValueCommand<TDto> : BaseQuery, IRequest<TDto>, IBaseCommand where TDto : BaseDto
{
    public DeleteValueCommand(TDto ValueDto, int UserId)
    {
        this.ValueDto = ValueDto;
        this.UserId = UserId;
        Url = $"api/{GetModelName<TDto>()}/{ValueDto.Id}";
    }

    public object ValueDto { get; init; }
    public int UserId { get; init; }
    public string Url { get; init; }
}