using System.ComponentModel;

namespace Pilot.Contracts.Data.Enums;

public enum ProjectStatus
{
    [Description("Не выбрано")]
    NotSelected,
    [Description("В разработке")]
    Development,
    [Description("Создается")]
    Ready,
    [Description("Выкладывается")]
    Publish,
    [Description("Удалён")]
    Deleted
}