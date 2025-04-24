namespace Pilot.Contracts.Data.Enums;

[Flags]
public enum Permission
{
    All = 1,
    ViewTaskTable = 2,
    TaskClosing = 4,
    ViewCompanyManagement = 8,
    ViewHrTable = 16,
    CreateTask = 32,
}