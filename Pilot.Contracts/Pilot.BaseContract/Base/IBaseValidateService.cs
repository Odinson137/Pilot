﻿namespace Pilot.Contracts.Base;

public interface IBaseValidatorService
{
    public Task ValidateAsync<T, TDto>(TDto model, int userId,
        bool canUserValidate = true, bool canDefaultValidate = true)
        where T : BaseModel where TDto : BaseDto;

    public Task ValidateAsync<T, TDto, TLocalUser>(TDto model, int userId,
        bool canUserValidate = true, bool canDefaultValidate = true, bool canLocalUserValidate = true)
        where T : BaseModel where TDto : BaseDto where TLocalUser : BaseModel;
    
    public Task FillValidateAsync<T>(T model) where T : BaseModel;

    public Task DeleteValidateAsync<T>(T model) where T : BaseModel;
}