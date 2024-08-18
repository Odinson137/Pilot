namespace Pilot.Identity.Commands;

public record MessengerUserCreated(int UserId) : LocalUserCreated(UserId);