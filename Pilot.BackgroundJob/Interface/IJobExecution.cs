namespace Pilot.BackgroundJob.Interface;

public interface IJobExecution
{
    public Task RecurringJobExecution(int reminderId);
}