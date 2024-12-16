namespace Pilot.BackgroundJob.Interface;

public interface IJob
{
    public void AddNewRecurringJob(string key, int reminderId, Func<string?> cron);
}