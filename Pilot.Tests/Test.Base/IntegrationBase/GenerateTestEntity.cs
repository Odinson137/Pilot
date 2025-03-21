﻿using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Pilot.Contracts.Attributes;
using Pilot.Contracts.Base;
using Pilot.Contracts.Data.Enums;
using Pilot.Worker.Models;

namespace Test.Base.IntegrationBase;

public static class GenerateTestEntity
{
    /// <summary>
    ///     Create entities
    /// </summary>
    /// <param name="type">Тип сущности</param>
    /// <param name="listDepth">Глубина создания дочерний сущностей</param>
    /// <param name="count">Количество создаваемых сущностей</param>
    /// <param name="listElementCount">Количество элементов в коллекциях</param>
    /// <returns></returns>
    public static ICollection<BaseModel> CreateEntities(Type type, int listDepth = 1, int count = 1,
        int listElementCount = 3)
    {
        var collection = new List<BaseModel>();
        for (var i = 0; i < count; i++)
        {
            var entity = (BaseModel)CreateEntity(Activator.CreateInstance(type)!, type, listDepth, listElementCount);
            collection.Add(entity);
        }

        return collection;
    }

    /// <summary>
    ///     Create entities
    /// </summary>
    /// <param name="listDepth">Глубина создания дочерний сущностей</param>
    /// <param name="count">Количество создаваемых сущностей</param>
    /// <param name="listElementCount">Количество элементов в коллекциях</param>
    /// <typeparam name="T">Сущность</typeparam>
    /// <returns></returns>
    public static ICollection<T> CreateEntities<T>(int listDepth = 1, int count = 1, int listElementCount = 3)
    {
        var collection = new List<T>();
        var type = typeof(T);
        for (var i = 0; i < count; i++)
        {
            var entity = (T)CreateEntity(Activator.CreateInstance<T>()!, type, listDepth, listElementCount);
            collection.Add(entity);
        }

        return collection;
    }

    public static async Task FillChildren<T>(T model, DbContext context) where T : BaseModel
    {
        var type = typeof(T);
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            var modelType = property.PropertyType;
            var value = property.GetValue(model);
            if (value == null) continue;

            if (property.PropertyType.IsClass && modelType.IsSubclassOf(typeof(BaseModel)) &&
                modelType != typeof(CompanyUser))
            {
                var childModel = value ?? throw new NullReferenceException("Дочерний объект не найден");
                await context.AddAsync(childModel);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(modelType) && modelType != typeof(string) &&
                     modelType.GetGenericArguments().First().IsSubclassOf(typeof(BaseModel)))
            {
                if (value is IEnumerable enumerable)
                {
                    var enumerator = enumerable.GetEnumerator();
                    using var enumerator1 = enumerator as IDisposable;
                    if (!enumerator.MoveNext()) continue;
                }

                var childrenCollectionModel =
                    value ?? throw new NullReferenceException("Дочерняя коллекция не найдена");
                await context.AddRangeAsync(childrenCollectionModel);
            }
        }

        await context.SaveChangesAsync();
    }

    public static async Task FillImage<T, TDto>(T model, DbContext storageContext)
    {
        var dtoType = typeof(TDto);
        var modelType = typeof(T);
        
        var properties = dtoType.GetProperties();

        var modelProperties = modelType.GetProperties();
        foreach (var property in properties)
        {
            var hasFileAttr = property.GetCustomAttributes(typeof(FileAttribute), false).FirstOrDefault();
            if (hasFileAttr is not FileAttribute) continue;
            
            if (typeof(ICollection<string>).IsAssignableFrom(property.PropertyType))
            {
                var modelAtr = modelType.GetProperties().First(c => c.Name == property.Name);

                var fileIds = new List<string>();
                for (var i = 0; i < 2; i++)
                {
                    var file = new Pilot.Storage.Models.File
                    {
                        Name = Guid.NewGuid().ToString(),
                        Type = ".png",
                        Format = FileFormat.Image,
                        Size = 500
                    };
                    await storageContext.AddAsync(file);
                    await storageContext.SaveChangesAsync();
                    fileIds.Add(file.Name);
                }

                modelAtr.SetValue(model, fileIds);
            }
            else if (property.PropertyType == typeof(string))
            {
                var modelAtr = modelProperties.First(c => c.Name == property.Name);

                var file = new Pilot.Storage.Models.File
                {
                    Name = Guid.NewGuid().ToString(),
                    Type = ".png",
                    Format = FileFormat.Image,
                    Size = 250
                };
                await storageContext.AddAsync(file);
                await storageContext.SaveChangesAsync();

                modelAtr.SetValue(model, file.Name);
            }
        }
    }

    public static async Task FillImage<T, TDto>(ICollection<T> models, DbContext storageContext)
    {
        foreach (var model in models)
            await FillImage<T, TDto>(model, storageContext);
    }


    private static object CreateEntity(object entity, Type type, int listDepth, int listElementCount)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.SetMethod == null || property.Name.Contains("Id") || property == typeof(int) || property == typeof(int?)) continue;

            if (property.PropertyType == typeof(string))
            {
                property.SetValue(entity, $"{Guid.NewGuid()}");
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                property.SetValue(entity, DateTime.Now);
            }
            else if (property.PropertyType.IsClass)
            {
                var haveAttribute = property.IsDefined(typeof(RequiredAttribute), false);
                if (haveAttribute)
                {
                    var newEntity = Activator.CreateInstance(property.PropertyType);
                    if (newEntity != null)
                        property.SetValue(entity,
                            CreateEntity(newEntity, property.PropertyType, listDepth - 1, listElementCount));
                }
            }
            else if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
            {
                var listType = GetCollectionElementType(property.PropertyType);
                var collection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(listType))!;
                for (var i = 0; i < listElementCount; i++)
                {
                    if (!listType.IsClass || listType == typeof(string) || listType == typeof(int)) continue;
                    var newEntity = Activator.CreateInstance(listType);
                    if (newEntity != null && listDepth > 0)
                    {
                        collection?.Add(CreateEntity(newEntity, listType, listDepth - 1, listElementCount));
                        property.SetValue(entity, collection);
                    }
                }
            }
        }

        return entity;
    }

    private static Type GetCollectionElementType(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return type.GetGenericArguments()[0];
        }

        var face = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        return face?.GetGenericArguments()[0]!;
    }
}