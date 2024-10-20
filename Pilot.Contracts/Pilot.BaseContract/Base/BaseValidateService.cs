using System.Collections;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pilot.Contracts.Data.Enums;
using Pilot.Contracts.DTO.ModelDto;
using Pilot.Contracts.Exception;
using Pilot.Contracts.Services;
using Pilot.Contracts.Services.LogService;
using Pilot.Contracts.Validation;

namespace Pilot.Contracts.Base;

public abstract class BaseValidateService : IBaseValidatorService
{
    private readonly DbContext _context;
    private readonly ILogger<BaseValidateService> _logger;

    public BaseValidateService(ILogger<BaseValidateService> logger, DbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task ValidateAsync<T, TDto>(TDto model)
        where T : BaseModel where TDto : BaseDto
    {
        _logger.LogInformation($"Start validate model of {typeof(T).Name}");
        _logger.LogClassInfo(model);

        await DefaultValidateAsync<T, TDto>(model);

        _logger.LogInformation($"End validate model of {typeof(T).Name}");
    }
    
    public async Task ValidateAsync<T, TDto, TLocalUser>(TDto model, int userId,
        bool canUserValidate = true, bool canDefaultValidate = true, bool canLocalUserValidate = true)
        where T : BaseModel where TDto : BaseDto where TLocalUser : BaseModel
    {
        _logger.LogInformation($"Start validate model of {typeof(T).Name}");
        _logger.LogClassInfo(model);

        if (canDefaultValidate)
            await DefaultValidateAsync<T, TDto>(model);
        
        if (canLocalUserValidate)
            await LocalUserValidateAsync<T, TLocalUser>(userId);

        _logger.LogInformation($"End validate model of {typeof(T).Name}");
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

    public async Task<T> DeleteValidateAsync<T>(int modelId) where T : BaseModel
    {
        _logger.LogInformation($"Start delete validate model of {typeof(T).Name}");
        _logger.LogClassInfo(modelId);

        var model = await _context.Set<T>().FirstOrDefaultAsync(c => c.Id == modelId);
        if (model == null)
        {
            _logger.LogError($"Value '{typeof(T).Name}' with Id = {modelId} is not exist");

            var message = new InfoMessageDto
            {
                Title = "Невозможно удалить",
                Description =
                    $"При попытке удалить значение {typeof(T).Name}' с Id = {modelId} произошла ошибка: сущность не была найдена",
                MessagePriority = MessageInfo.Error | MessageInfo.Delete | MessageInfo.Validate,
                EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
                EntityId = modelId
            };

            throw new MessageException(message);
        }

        return model;
    }

    private async Task DefaultValidateAsync<T, TDto>(TDto model) where T : BaseModel where TDto : BaseDto
    {
        var isValidate = await _context.Set<T>().Validate(model);

        if (isValidate.IsNotSuccessfully)
        {
            _logger.LogError($"{typeof(T).Name} has error: \n{isValidate.Error}");

            var message = new InfoMessageDto
            {
                Title = "Ошибка валидации",
                Description = isValidate.Error,
                MessagePriority = MessageInfo.Error | MessageInfo.Validate,
                EntityType = PilotEnumExtensions.GetModelEnumValue<T>(),
                EntityId = model.Id
            };

            throw new MessageException(message);
        }
    }

    private async Task LocalUserValidateAsync<T, TLocalUser>(int userId) where T : BaseModel where TLocalUser : BaseModel
    {
        var companyUser = await _context.Set<TLocalUser>().Where(c => c.Id == userId).AnyAsync();
        if (!companyUser) // Позже добавить ещё проверку на роль пользователя
        {
            _logger.LogError("User is not found");

            var message = new InfoMessageDto
            {
                Title = "Ошибка валидации",
                Description = "Данный локальный пользователь не найден. Попробуйте позже",
                MessagePriority = MessageInfo.Error | MessageInfo.Validate,
                EntityType = PilotEnumExtensions.GetModelEnumValue<T>()
            };

            throw new MessageException(message);
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

        var message = new InfoMessageDto
        {
            Title = "Ошибка связанной сущности",
            Description =
                $"Вы пытаетесь добавить/обновить значение ({property.PropertyType.Name} - {property.Name}), которое не существует",
            MessagePriority = MessageInfo.Error | MessageInfo.Update | MessageInfo.Validate,
            EntityType = PilotEnumExtensions.GetModelEnumValue(propertyType.Name),
            EntityId = valueId
        };

        throw new MessageException(message);
    }
}