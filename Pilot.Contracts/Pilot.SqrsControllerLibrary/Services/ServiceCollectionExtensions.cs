using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;
using Pilot.SqrsControllerLibrary.Handlers;
using Pilot.SqrsControllerLibrary.Queries;

namespace Pilot.SqrsControllerLibrary.Services;

public static class ServiceCollectionExtensions
{
    public static void AddBaseQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        var dtoTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(BaseDto).IsAssignableFrom(t))
            .ToList();

        var modelTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(BaseModel).IsAssignableFrom(t))
            .ToList();
        
        #region CRUD handlers

        var handlerInterfaceTypeWithRequest = typeof(IRequestHandler<,>);

        AddGetByIdQueryHandlers(services, dtoTypes, modelTypes, handlerInterfaceTypeWithRequest);
        AddGetValuesQueryHandlers(services, dtoTypes, modelTypes, handlerInterfaceTypeWithRequest);
        #endregion
    }

    private static void AddGetByIdQueryHandlers(IServiceCollection services, IList<Type> dtoTypes, IList<Type> modelTypes, Type handlerInterfaceType)
    {
        var queryType = typeof(GetValueByIdQuery<>);
        var handlerType = typeof(GetValueQueryHandler<,>);

        for (var i = 0; i < dtoTypes.Count; i++)
        {
            var dtoType = dtoTypes[i];
            var modelType = modelTypes[i];
            
            var requestType = queryType.MakeGenericType(dtoType);
            var genericHandlerInterfaceType = handlerInterfaceType.MakeGenericType(requestType, dtoType);
            var genericHandlerType = handlerType.MakeGenericType(dtoType, modelType);

            services.AddScoped(genericHandlerInterfaceType, genericHandlerType);
        }
    }
    
    private static void AddGetValuesQueryHandlers(IServiceCollection services, IList<Type> dtoTypes, IList<Type> modelTypes, Type handlerInterfaceType)
    {
        var queryType = typeof(GetValuesQuery<>);
        var handlerType = typeof(GetValuesQueryHandler<,>);

        for (var i = 0; i < dtoTypes.Count; i++)
        {
            var dtoType = dtoTypes[i];
            var modelType = modelTypes[i];
            
            var requestType = queryType.MakeGenericType(dtoType);
            var responseType = typeof(ICollection<>).MakeGenericType(dtoType);
            var genericHandlerInterfaceType = handlerInterfaceType.MakeGenericType(requestType, responseType);
            var genericHandlerType = handlerType.MakeGenericType(dtoType, modelType);

            services.AddScoped(genericHandlerInterfaceType, genericHandlerType);
        }
    }
}