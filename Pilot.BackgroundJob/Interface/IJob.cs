namespace Pilot.BackgroundJob.Interface;

public interface IJob
{
    public void ReminderRecurringJobExecution(string key, int reminderId, Func<string?> cron);
}