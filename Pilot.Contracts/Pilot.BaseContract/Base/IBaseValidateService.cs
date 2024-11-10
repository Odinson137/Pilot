namespace Pilot.Contracts.Base;

public interface IBaseValidatorService
{
    public Task ValidateAsync<T, TDto>(TDto model)
        where T : BaseModel where TDto : BaseDto;
    
    public Task FillValidateAsync<T>(T model) where T : BaseModel;

    public Task<T> DeleteValidateAsync<T>(int modelId) where T : BaseModel;
}