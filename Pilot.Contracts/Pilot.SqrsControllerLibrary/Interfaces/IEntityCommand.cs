using MediatR;
using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Interfaces;

public interface IEntityCommand<T> : IRequest<BaseModel> where T : notnull
{
    public T Value { get; init; }
    public int UserId { get; init; }
}