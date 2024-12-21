namespace Pilot.BackgroundJob.Interface;

public interface IJob
{
    public void AddReminderRecurringJob(string key, int reminderId, string cronExpression);
    public void RemoveReminderRecurringJob(string key);
}