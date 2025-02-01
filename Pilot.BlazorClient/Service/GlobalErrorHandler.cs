using Microsoft.AspNetCore.Components.Web;

namespace Pilot.BlazorClient.Service;

using Microsoft.Extensions.Logging;
using Blazored.Toast.Services;

public class GlobalErrorHandler : IErrorBoundaryLogger
{
    private readonly ILogger<GlobalErrorHandler> _logger;
    private readonly IToastService _toastService;

    public GlobalErrorHandler(ILogger<GlobalErrorHandler> logger, IToastService toastService)
    {
        _logger = logger;
        _toastService = toastService;
    }

    public ValueTask LogErrorAsync(Exception exception)
    {
        // Логируем ошибку
        _logger.LogError(exception, "Произошла глобальная ошибка");

        // Показываем уведомление пользователю
        _toastService.ShowError("Произошла ошибка. Пожалуйста, попробуйте снова.");

        return ValueTask.CompletedTask;
    }
}