using Pilot.Contracts.Base;
using Pilot.Contracts.Validation;

namespace Pilot.Receiver.Interface;

public interface IValidatorService
{
    public Task<ValidateError> Validate<T, TDto>(TDto model, int userId) where T : BaseModel where TDto : BaseDto;
    
    public Task<ValidateError> UpdateValidate<T>(T model) where T : BaseModel;
}