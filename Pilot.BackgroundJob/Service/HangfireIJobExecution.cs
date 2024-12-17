using AutoMapper;
using Hangfire;
using Pilot.BackgroundJob.Interface;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Interfaces;

namespace Pilot.BackgroundJob.Service;

public class HangfireJobExecution : IJob
{
    private readonly IMessageService _messageService;
    private readonly IChatReminder _chatReminder;
    private readonly ILogger<HangfireJobExecution> _logger;

    public HangfireJobExecution(IMessageService messageService, IChatReminder chatReminder,
        ILogger<HangfireJobExecution> logger)
    {
        _messageService = messageService;
        _chatReminder = chatReminder;
        _logger = logger;
    }

    public async void ReminderRecurringJobExecution(string key, int reminderId, Func<string?> cron)
    {
        var reminder = await _chatReminder.GetByIdAsync(reminderId);
        if (reminder == null)
        {
            _logger.LogCritical("Something went wrong getting the reminder. Is the reminder id null?");
            return;
        }

        var message = new MessageDto
        {
            Text = reminder.Message,
            UserId = (int)ChatMemberId.Reminder,
            Chat = new BaseDto { Id = reminder.ChatId }
        };

        await _messageService.SendMessageChatAsync(message);
    }
}