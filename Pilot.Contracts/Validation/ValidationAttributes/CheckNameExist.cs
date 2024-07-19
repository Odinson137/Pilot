using System.Linq.Expressions;
using System.Reflection;
using Amazon.Runtime.Internal.Util;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Validation.ValidationAttributes;

[AttributeUsage( AttributeTargets.All )]
public class CheckNameExist : Attribute, IValidationAttribute
{
    public async Task<ValidateError> IsValid<T, TDto>(PropertyInfo propertyInfo, TDto model, DbSet<T> dbSet) where T : BaseModel where TDto : BaseDto
    {
        var name = propertyInfo.Name;
        var value = (string)propertyInfo.GetValue(model)!;

        var parameter = Expression.Parameter(typeof(T), "model");
        var equalValue = Expression.Constant(value);
        var property = Expression.Property(parameter, name);
        var equalExpression = Expression.Equal(property, equalValue);

        var lambda = Expression.Lambda<Func<T, bool>>(equalExpression, [parameter]);

        var isFound = await dbSet.AnyAsync(lambda);

        return isFound 
            ? new ValidateError($"Ошибка при добавлении значения '{value}' модели '{typeof(T).Name}'! Оно должно быть уникально во всей системе.") 
            : new ValidateError();
    }
    
    public Task<ValidateError> IsValid<T, TDto>(PropertyInfo propertyInfo, TDto model, IBaseReadRepository<T> context) where T : BaseModel where TDto : BaseDto
    {
        return IsValid(propertyInfo, model, context.DbSet);
    }
}