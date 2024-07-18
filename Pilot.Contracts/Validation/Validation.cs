using System.Reflection;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Validation;

public static class Validation
{
    // public static async Task<AttributeError> Validate<T1, T2>(this BaseReadRepository<T1> context, params T2[] models) where T1 : BaseModel where T2 : BaseDto
    // {
    //     foreach (var model in models)
    //     {
    //         var attributeError  = context.Validate(model);
    //         if (!(await attributeError).IsSuccessfully)
    //         {
    //             return attributeError;
    //         }
    //     }
    //
    //     return new AttributeError();
    // }
    
    public static async Task<AttributeError> Validate<T1, T2>(this IBaseReadRepository<T1> context, T2 model) where T1 : BaseModel where T2 : BaseDto
    {
        var validationType = typeof(IValidationAttribute);
        var assembly = Assembly.GetAssembly(validationType);
        
        var customAttributes = assembly?.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && validationType.IsAssignableFrom(t))
            .ToList() ?? [];
        
        var type = typeof(T1);

        var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var field in fields)
        {
            var foundAttributes = field.GetCustomAttributes()
                .Where(c => customAttributes.Contains(c.GetType()))
                .Select(c => (IValidationAttribute)c)
                .ToList();

            foreach (var foundAttribute in foundAttributes)
            {
                var attributeError = await foundAttribute.IsValid(field, model, context);
                if (attributeError.IsNotSuccessfully)
                {
                    return attributeError;
                }
            }
        }

        return new AttributeError();
    }
}