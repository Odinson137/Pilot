using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Base;

namespace Pilot.Contracts.Validation;

public static class Validation
{
    public static ValidateError DefaultValidate<TDto>(this TDto model) where TDto : BaseDto
    {
        var validationContext = new ValidationContext(model);
        var results = new List<ValidationResult>();

        if (Validator.TryValidateObject(model, validationContext, results, true))
            return new ValidateError();

        var builder = new StringBuilder();
        builder.AppendLine("У вас есть несколько проблем при валидации ваших значений перед её сохранением в базу:");
        foreach (var result in results)
        {
            builder.Append(result.ErrorMessage);
            builder.AppendLine();
        }

        builder.Remove(builder.Length - Environment.NewLine.Length, Environment.NewLine.Length);

        return new ValidateError(builder.ToString());
    }

    public static async Task<ValidateError> Validate<T, TDto>(this DbSet<T> context, TDto model)
        where T : BaseModel where TDto : BaseDto
    {
        var defaultValidate = DefaultValidate(model);
        if (defaultValidate.IsNotSuccessfully) return defaultValidate;

        var validationType = typeof(IValidationAttribute);
        var assembly = Assembly.GetAssembly(validationType);

        var customAttributes = assembly?.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && validationType.IsAssignableFrom(t))
            .ToList() ?? [];

        var type = typeof(T);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var foundAttributes = property.GetCustomAttributes()
                .Where(c => customAttributes.Contains(c.GetType()))
                .Select(c => (IValidationAttribute)c)
                .ToList();

            foreach (var foundAttribute in foundAttributes)
            {
                var attributeError = await foundAttribute.IsValid(property, model, context);
                if (attributeError.IsNotSuccessfully) return attributeError;
            }
        }

        return new ValidateError();
    }

    public static Task<ValidateError> Validate<T1, T2>(this IBaseReadRepository<T1> context, T2 model)
        where T1 : BaseModel where T2 : BaseDto
    {
        return Validate(context.DbSet, model);
    }
}