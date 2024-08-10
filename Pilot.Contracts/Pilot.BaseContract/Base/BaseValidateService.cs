using System.Collections;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Exception.ProjectExceptions;
using Pilot.Contracts.Interfaces;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.Contracts.Validation;

namespace Pilot.Contracts.Base;

public abstract class BaseValidateService<TLocalUser> : IBaseValidatorService where TLocalUser : BaseModel
{
    private readonly IMessageService _message;
    private readonly IUserService _user;
    private readonly ILogger<ValidateError> _logger;
    private readonly DbContext _context;

    public BaseValidateService(IMessageService message, IUserService user, ILogger<ValidateError> logger, DbContext context)
    {
        _message = message;
        _user = user;
        _logger = logger;
        _context = context;
    }

    public async Task ValidateAsync<T, TDto>(TDto model, int userId, 
        bool canUserValidate = true, bool canDefaultValidate = true, bool canCompanyUserValidate = true) 
        where T : BaseModel where TDto : BaseDto
    {
        _logger.LogInformation($"Start validate model of {typeof(T).Name}");
        _logger.LogClassInfo(model);

        if (canUserValidate)
            await UserValidateAsync(userId);

        if (canDefaultValidate)
            await DefaultValidateAsync<T, TDto>(model);

        if (canCompanyUserValidate)
            await LocalUserValidateAsync<T>(userId);
        
        _logger.LogInformation($"End validate model of {typeof(T).Name}");
    }

    private async Task UserValidateAsync(int userId)
    {
        var user = await _user.GetValueByIdAsync(userId);

        // по логике, это условие всегда должно быть положительным, если система консистентна, иначе она не допустит появлению не связанных данных
        if (user == null)
        {
            _logger.LogError("User not found");
            throw new NotFoundException("User not found");
        }
    }

    private async Task DefaultValidateAsync<T, TDto>(TDto model) where T : BaseModel where TDto : BaseDto
    {
        var isValidate = await _context.Set<T>().Validate(model);

        if (isValidate.IsNotSuccessfully)
        {
            _logger.LogError($"{typeof(T).Name} has error: \n{isValidate.Error}");
            
            var message = new MessageDto
            {
                Title = "Ошибка валидации",
                Description = isValidate.Error,
                MessagePriority = MessagePriority.Error | MessagePriority.Validate,
                EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
                EntityId = model.Id
            };
            
            await _message.SendMessage(message);
            throw new BadRequestException($"{isValidate.Error}");
        }
    }

    private async Task LocalUserValidateAsync<T>(int userId) where T : BaseModel
    {
        var companyUser = await _context.Set<TLocalUser>().Where(c => c.Id == userId).AnyAsync();
        if (!companyUser) // Позже добавить ещё проверку на роль пользователя
        {
            _logger.LogError("Company user is not found");
            
            var message = new MessageDto
            {
                Title = "Ошибка валидации",
                Description = "Данный пользователь в компании не найден. Пройдите регистрацию или попробуйте позже",
                MessagePriority = MessagePriority.Error | MessagePriority.Validate,
                EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
            };
            
            await _message.SendMessage(message);
            throw new NotFoundException($"{typeof(T).Name} has no exist");
        }
    }

    public async Task FillValidateAsync<T>(T model) where T : BaseModel
    {
        _logger.LogInformation($"Start fill validate model of {typeof(T).Name}");
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
                        if (subModel == null) continue;
                        
                        updatedCollection!.Add(subModel);
                    }

                    property.SetValue(model, updatedCollection);
                }
            }
        }
    }

    public async Task DeleteValidateAsync<T>(T model) where T : BaseModel
    {
        _logger.LogInformation($"Start delete validate model of {typeof(T).Name}");
        _logger.LogClassInfo(model);

        var anyModelExist = await _context.Set<T>().AnyAsync(c => c.Id == model.Id);
        if (!anyModelExist)
        {
            _logger.LogError($"Value '{typeof(T).Name}' with Id = {model.Id} is not exist");
        
            var message = new MessageDto
            {
                Title = "Невозможно удалить",
                Description =  $"При попытке удалить значение {typeof(T).Name}' с Id = {model.Id} произошла ошибка: сущность не была найдена",
                MessagePriority = MessagePriority.Error | MessagePriority.Delete | MessagePriority.Validate,
                EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
                EntityId = model.Id
            };
            
            await _message.SendMessage(message);
        } 
    }

    private async ValueTask<object?> GetSubEntity(Type propertyType, PropertyInfo property, object value)
    {
        var valueId = ((BaseModel)value).Id;
        if (valueId == 0) return null;
        
        var subModel = await _context.FindAsync(propertyType, valueId);

        if (subModel != null) return subModel;
        
        _logger.LogError(
            $"Property {property.PropertyType.Name} - {property.Name} has BAD id that is not contained in db");
        
        var message = new MessageDto
        {
            Title = "Ошибка связанной сущности",
            Description =  $"Вы пытаетесь добавить/обновить значение ({property.PropertyType.Name} - {property.Name}), которое не существует",
            MessagePriority = MessagePriority.Error | MessagePriority.Update | MessagePriority.Validate,
            EntityType = PilotEnumExtensions.GetModelEnumValue(propertyType.Name),
            EntityId = valueId
        };
            
        await _message.SendMessage(message);

        throw new NotFoundException(
            $"Property {property.PropertyType.Name} - {property.Name} has BAD id that is not contained in db");

    }
}