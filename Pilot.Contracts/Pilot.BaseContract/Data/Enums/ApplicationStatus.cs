namespace Pilot.Contracts.Data.Enums;

public enum ApplicationStatus
{
    Pending,    // Ожидание
    Reviewing,    // Взятие вакансии в дело (здесь надо создать чат)
    Approved,   // Принята
    Canceled,   // Отклонена (самим юзером)
    Rejected    // Отклонена (работодателем)
}