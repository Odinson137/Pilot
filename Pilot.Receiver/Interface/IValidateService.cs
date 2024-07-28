using Pilot.Contracts.Base;

namespace Pilot.Receiver.Interface;

public interface IValidatorService
{
    public Task ValidateAsync<T, TDto>(TDto model, int userId) where T : BaseModel where TDto : BaseDto;

    Task ValidateWithoutDefaultAsync<T, TDto>(TDto model, int userId) where T : BaseModel where TDto : BaseDto;
    
    public Task UpdateValidateAsync<T>(T model) where T : BaseModel;
    
    public Task DeleteValidateAsync<T>(T model) where T : BaseModel;
}