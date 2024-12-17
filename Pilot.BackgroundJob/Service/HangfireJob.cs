using System.Linq.Expressions;
using Hangfire;
using Pilot.BackgroundJob.Interface;

namespace Pilot.BackgroundJob.Service;

public class HangfireJob : IJob
{
    private readonly IRecurringJobManager _recurringJobManager;

    public HangfireJob(IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    public void ReminderRecurringJobExecution(string key, int reminderId, Func<string?> cron)
    {
        _recurringJobManager.AddOrUpdate(key, () => Console.WriteLine("пока так"), cron);
    }
    
    public void AddRecurringJob(string key, Expression<Action> action, Func<string?> cron)
    {
        _recurringJobManager.AddOrUpdate(key, action, cron);
    }
}