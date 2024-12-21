using System.Linq.Expressions;
using Hangfire;
using Pilot.BackgroundJob.Interface;

namespace Pilot.BackgroundJob.Service;

public class HangfireJob : IJob
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IJobExecution _jobExecution;

    public HangfireJob(IRecurringJobManager recurringJobManager, IJobExecution jobExecution)
    {
        _recurringJobManager = recurringJobManager;
        _jobExecution = jobExecution;
    }

    public void AddReminderRecurringJob(string key, int reminderId, string cronExpression)
    {
        _recurringJobManager.AddOrUpdate(
            key, 
            () => _jobExecution.RecurringJobExecution(reminderId), 
            cronExpression);
    }

    public void RemoveReminderRecurringJob(string key)
    {
        _recurringJobManager.RemoveIfExists(key);
    }

    // наверное делать такую абстракцию как по мне глупо. Смысла в сервисе тогда не будет
    // уж лучше буду делать так, как в верхнем методе. Для каждого дейссвтия свой метод
    public void AddRecurringJob(string key, Expression<Action> action, string cronExpression)
    {
        _recurringJobManager.AddOrUpdate(key, action, cronExpression);
    }
}