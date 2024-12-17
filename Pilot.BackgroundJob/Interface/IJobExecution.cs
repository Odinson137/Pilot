namespace Pilot.BackgroundJob.Interface;

public interface IJobExecution
{
    public void RecurringJob(string key, int reminderId, Func<string?> cron);
}