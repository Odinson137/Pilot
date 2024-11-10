using Pilot.Contracts.Base;
using Pilot.Contracts.Services.LogService;
using Pilot.Worker.Data;
using Pilot.Worker.Interface;

namespace Pilot.Worker.Service;

public class ValidatorService(
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(logger, context), IValidatorService
{
        
    public async Task ValidateAsync<T, TDto, TLocalUser>(TDto model, int userId,
        bool canUserValidate = true, bool canDefaultValidate = true, bool canLocalUserValidate = true)
        where T : BaseModel where TDto : BaseDto where TLocalUser : BaseModel
    {
        logger.LogInformation($"Start validate model of {typeof(T).Name}");
        logger.LogClassInfo(model);

        if (canDefaultValidate)
            await DefaultValidateAsync<T, TDto>(model);
        
        if (canLocalUserValidate)
            await LocalUserValidateAsync<T, TLocalUser>(userId);

        logger.LogInformation($"End validate model of {typeof(T).Name}");
    }
}