using System.ComponentModel;

namespace Pilot.Contracts.Data.Enums;

[Flags]
public enum Permission
{
    CreateTask = 1,
    ViewTaskTable = 2,
    TaskClosing = 4,
}