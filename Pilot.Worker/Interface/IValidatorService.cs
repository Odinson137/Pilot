using Pilot.Contracts.Base;

namespace Pilot.Worker.Interface;

public interface IValidatorService : IBaseValidatorService
{
    
    public Task ValidateAsync<T, TDto, TLocalUser>(TDto model, int userId,
        bool canUserValidate = true, bool canDefaultValidate = true, bool canLocalUserValidate = true)
        where T : BaseModel where TDto : BaseDto where TLocalUser : BaseModel;
}