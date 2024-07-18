using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Validation.ValidationAttributes;

[AttributeUsage( AttributeTargets.All )]
public class CheckNameExist : Attribute, IValidationAttribute
{
    public async Task<AttributeError> IsValid<T, TDto>(PropertyInfo propertyInfo, TDto model, IBaseReadRepository<T> context) where T : BaseModel where TDto : BaseDto
    {
        var name = propertyInfo.Name;
        var value = (string)propertyInfo.GetValue(model)!;

        var parameter = Expression.Parameter(typeof(T), "model");
        var equalValue = Expression.Constant(value);
        var property = Expression.Property(parameter, name);
        var equalExpression = Expression.Equal(property, equalValue);

        var lambda = Expression.Lambda<Func<T, bool>>(equalExpression, [parameter]);

        var isFound = await context.DbSet.AnyAsync(lambda);

        return isFound 
            ? new AttributeError($"Ошибка при добавлении значения '{value}' модели '{typeof(T).Name}'! Оно должно быть уникально во всей системе.") 
            : new AttributeError();
    }
}