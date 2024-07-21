using Pilot.Contracts.Base;

namespace Pilot.Receiver.Interface;

public interface IValidatorService
{
    public Task Validate<T, TDto>(TDto model, int userId) where T : BaseModel where TDto : BaseDto;
    
    public Task UpdateValidate<T>(T model) where T : BaseModel;
}