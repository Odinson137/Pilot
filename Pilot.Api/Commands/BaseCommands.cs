using MediatR;
using Pilot.Api.Data;
using Pilot.Contracts.Base;

namespace Pilot.Api.Commands;

public record AddValueCommand<TDto> : BaseQuery, IRequest<TDto>, IBaseCommand where TDto : BaseDto
{
    public AddValueCommand(TDto ValueDto, string UserId)
    {
        this.ValueDto = ValueDto;
        this.UserId = UserId;
        Url = $"api/{GetModelName<TDto>()}";
    }

    public object ValueDto { get; init; }
    public string UserId { get; init; }
    public string Url { get; init; }
}

public record UpdateValueCommand<TDto> : BaseQuery, IRequest<TDto>, IBaseCommand where TDto : BaseDto
{
    public UpdateValueCommand(TDto ValueDto, string UserId)
    {
        this.ValueDto = ValueDto;
        this.UserId = UserId;
        Url = $"api/{GetModelName<TDto>()}";
    }

    public object ValueDto { get; init; }
    public string UserId { get; init; }
    public string Url { get; init; }
}

public record DeleteValueCommand<TBaseDto> : BaseQuery, IRequest<TBaseDto>, IBaseCommand where TBaseDto : BaseDto
{
    public DeleteValueCommand(TBaseDto ValueDto, string UserId)
    {
        this.ValueDto = ValueDto;
        this.UserId = UserId;
        Url = $"api/{GetModelName<TBaseDto>()}/{ValueDto.Id}";
    }

    public object ValueDto { get; init; }
    public string UserId { get; init; }
    public string Url { get; init; }
}