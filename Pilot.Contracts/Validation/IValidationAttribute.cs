using System.Reflection;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Validation;

public interface IValidationAttribute
{
    Task<AttributeError> IsValid<T, TDto>(PropertyInfo propertyInfo, TDto model, IBaseReadRepository<T> context) 
        where T : BaseModel where TDto : BaseDto;
}