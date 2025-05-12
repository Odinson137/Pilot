using MediatR;

namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface ICommand<TDto> : IRequest
{
    public TDto ValueDto { get; init; }
    
    public int UserId { get; init; }
}