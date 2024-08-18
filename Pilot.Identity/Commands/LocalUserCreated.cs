using MediatR;

namespace Pilot.Identity.Commands;

public abstract record LocalUserCreated(int UserId) : INotification;
