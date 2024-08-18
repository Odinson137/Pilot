namespace Pilot.Identity.Commands;

public record CompanyUserCreated(int UserId) : LocalUserCreated(UserId);