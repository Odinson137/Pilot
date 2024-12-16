using Hangfire;
using Pilot.BackgroundJob.Interface;
using Pilot.Contracts.Interfaces;

namespace Pilot.BackgroundJob.Service;

public class HangfireIJob : IJob
{
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly IMessageService _messageService; // создать промежуточный сервис

    public HangfireIJob(IRecurringJobManager recurringJobManager, IMessageService messageService)
    {
        _recurringJobManager = recurringJobManager;
        _messageService = messageService;
    }

    public void AddNewRecurringJob(string key, int reminderId, Func<string?> cron)
    {
        _recurringJobManager.AddOrUpdate(key, () => Console.WriteLine("пока так"), cron);
    }
}