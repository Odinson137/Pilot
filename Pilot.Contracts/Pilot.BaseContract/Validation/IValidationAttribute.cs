using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Validation;

public interface IValidationAttribute
{
    Task<ValidateError> IsValid<T, TDto>(PropertyInfo propertyInfo, TDto model, DbSet<T> dbSet)
        where T : BaseModel where TDto : BaseDto;

    Task<ValidateError> IsValid<T, TDto>(PropertyInfo propertyInfo, TDto model, IBaseReadRepository<T> context)
        where T : BaseModel where TDto : BaseDto;
}