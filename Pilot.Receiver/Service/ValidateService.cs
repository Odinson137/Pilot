using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Models;
using Pilot.Contracts.Services.LogService;
using Pilot.Contracts.Validation;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class ValidateService : IValidateService
{
    private readonly IMessage _message;
    private readonly IUserService _user;
    private readonly ILogger<ValidateError> _logger;
    private readonly DataContext _context;

    public ValidateService(IMessage message, IUserService user, ILogger<ValidateError> logger, DataContext context)
    {
        _message = message;
        _user = user;
        _logger = logger;
        _context = context;
    }

    public async Task<ValidateError> Validate<T, TDto>(TDto model, string userId) where T : BaseModel where TDto : BaseDto
    {
        _logger.LogInformation($"Start validate model of {typeof(T).Name}");
        _logger.LogClassInfo(model);
        
        var isValidate = await _context.Set<T>().Validate(model);

        if (isValidate.IsNotSuccessfully)
        {
            _logger.LogInformation("Company has already existed");
            await _message.SendMessage("Ошибка в создании", isValidate.Error, MessagePriority.Error);
            throw new BadRequestException("Company has already existed");
        }
        
        var user = await _user.GetUserByIdAsync(userId);

        if (user == null)
        {
            _logger.LogInformation("User not found");
            throw new NotFoundException("User not found");
        }
        
        _logger.LogInformation($"End validate model of {typeof(T).Name}");
        return new ValidateError();
    }
}