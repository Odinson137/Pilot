using Pilot.Contracts.Base;
using Pilot.Identity.Data;
using Pilot.Identity.Interfaces;

namespace Pilot.Identity.Services;

public class ValidatorService(
    ILogger<ValidatorService> logger,
    DataContext context)
    : BaseValidateService(logger, context)
{
        
    // public async Task ValidateAsync<T, TDto, TLocalUser>(TDto model, int userId,
    //     bool canUserValidate = true, bool canDefaultValidate = true, bool canLocalUserValidate = true)
    //     where T : BaseModel where TDto : BaseDto where TLocalUser : BaseModel
    // {
    //     logger.LogInformation($"Start validate model of {typeof(T).Name}");
    //     logger.LogClassInfo(model);
    //
    //     if (canDefaultValidate)
    //         await DefaultValidateAsync<T, TDto>(model);
    //     
    //     // TODO потом делать запросы в сервис с юзером и проверять всю валидность через него, включая роль
    //     // if (canLocalUserValidate)
    //     //     await LocalUserValidateAsync<T, TLocalUser>(userId);
    //
    //     logger.LogInformation($"End validate model of {typeof(T).Name}");
    // }
}