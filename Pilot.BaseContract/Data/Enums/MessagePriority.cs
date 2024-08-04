namespace Pilot.Contracts.Data.Enums;

[Flags]
public enum MessagePriority
{
    Default = 0,
    Error = 1,
    Job = 2,
    Invitation = 4,
    Success = 8,
    Create = 16,
    Update = 32,
    Delete = 64,
    Validate = 128,
}