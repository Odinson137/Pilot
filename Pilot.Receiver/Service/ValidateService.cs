using System.Collections;
using System.Reflection;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Models;
using Pilot.Contracts.Services.LogService;
using Pilot.Contracts.Validation;
using Pilot.Receiver.Data;
using Pilot.Receiver.Interface;

namespace Pilot.Receiver.Service;

public class ValidatorService : IValidatorService
{
    private readonly IMessage _message;
    private readonly IUserService _user;
    private readonly ILogger<ValidateError> _logger;
    private readonly DataContext _context;

    public ValidatorService(IMessage message, IUserService user, ILogger<ValidateError> logger, DataContext context)
    {
        _message = message;
        _user = user;
        _logger = logger;
        _context = context;
    }

    public async Task Validate<T, TDto>(TDto model, int userId) where T : BaseModel where TDto : BaseDto
    {
        _logger.LogInformation($"Start validate model of {typeof(T).Name}");
        _logger.LogClassInfo(model);
        
        var user = await _user.GetValueByIdAsync(userId);

        // по логике, это условие всегда должно быть положительным, если система консистентна, иначе она не допустит появлению не связанных данных
        if (user == null)
        {
            _logger.LogError("User not found");
            throw new NotFoundException("User not found");
        }
        
        var companyUser = await _context.Set<CompanyUser>().Where(c => c.Id == userId).AnyAsync();
        if (!companyUser) // Позже добавить ещё проверку на роль пользователя
        {
            _logger.LogError($"Company user is not found");
            await _message.SendMessage("Ошибка валидации", "Данный пользователь не найден. Попробуйте позже", MessagePriority.Error | MessagePriority.Validate);
            throw new NotFoundException($"{typeof(T).Name} has already existed");
        }
        
        var isValidate = await _context.Set<T>().Validate(model);

        if (isValidate.IsNotSuccessfully)
        {
            _logger.LogError($"{typeof(T).Name} has already existed");
            await _message.SendMessage("Ошибка валидации", isValidate.Error, MessagePriority.Error | MessagePriority.Validate);
            throw new BadRequestException($"{typeof(T).Name} has already existed");
        }
        
        _logger.LogInformation($"End validate model of {typeof(T).Name}");
    }

    public async Task UpdateValidate<T>(T model) where T : BaseModel
    {
        _logger.LogInformation($"Start update validate model of {typeof(T).Name}");
        _logger.LogClassInfo(model);

        var type = typeof(T);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            if (typeof(BaseModel).IsAssignableFrom(propertyType))
            {
                var value = property.GetValue(model);
                if (value == null) continue;

                var subModel = await GetSubEntity(propertyType, property, value);

                property.SetValue(model, subModel);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(propertyType) && propertyType.IsGenericType)
            {
                var elementType = propertyType.GetGenericArguments()[0];
                if (typeof(BaseModel).IsAssignableFrom(elementType))
                {
                    var collection = property.GetValue(model) as IEnumerable;
                    if (collection == null) continue;

                    var updatedCollection =
                        Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)) as IList;

                    foreach (var item in collection)
                    {
                        var subModel = await GetSubEntity(elementType, property, (BaseModel)item);
                        updatedCollection!.Add(subModel);
                    }

                    property.SetValue(model, updatedCollection);
                }
            }
        }
    }

    private async Task<object> GetSubEntity(Type propertyType, PropertyInfo property, object value)
    {
        var subModel = await _context.FindAsync(propertyType, ((BaseModel)value).Id);

        if (subModel != null) return subModel;
        
        _logger.LogError(
            $"Property {property.PropertyType.Name} - {property.Name} has BAD id that is not contained in db");
        await _message.SendMessage("Ошибка связанной сущности", 
            "Вы пытаетесь добавить/обновить значение, которое не существует",
            MessagePriority.Error | MessagePriority.Update | MessagePriority.Validate);

        throw new NotFoundException(
            $"Property {property.PropertyType.Name} - {property.Name} has BAD id that is not contained in db");

    }
}