using System.Reflection;
using MediatR;
using Pilot.Api.Commands;
using Pilot.Api.Handlers;
using Pilot.Contracts.Base;
using Pilot.Contracts.DTO;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.Api.Services;

public static class ServiceCollectionExtensions
{
    public static void AddQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        var dtoTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(BaseDto).IsAssignableFrom(t))
            .ToList();

        #region CRUD handlers

        var handlerInterfaceTypeWithRequest = typeof(IRequestHandler<,>);

        AddGetByIdQueryHandlers(services, dtoTypes, handlerInterfaceTypeWithRequest);
        AddGetValuesQueryHandlers(services, dtoTypes, handlerInterfaceTypeWithRequest);

        var handlerInterfaceType = typeof(IRequestHandler<>);
        
        AddCreateValueQueryHandlers(services, dtoTypes, handlerInterfaceType);
        AddUpdateValueQueryHandlers(services, dtoTypes, handlerInterfaceType);
        AddDeleteValueQueryHandlers(services, dtoTypes, handlerInterfaceType);

        #endregion

        #region User handlers

        services.AddScoped<IRequestHandler<UserRegistrationCommand>, UserCommandHandler>();
        services.AddScoped<IRequestHandler<UserAuthorizationCommand, AuthUserDto>, UserCommandHandler>();

        #endregion
    }

    private static void AddGetByIdQueryHandlers(IServiceCollection services, IReadOnlyCollection<Type> dtoTypes, Type handlerInterfaceType)
    {
        var queryType = typeof(GetValueByIdQuery<>);
        var handlerType = typeof(GetValueByIdQuery<>);

        foreach (var dtoType in dtoTypes)
        {
            var requestType = queryType.MakeGenericType(dtoType);
            var genericHandlerInterfaceType = handlerInterfaceType.MakeGenericType(requestType, dtoType);
            var genericHandlerType = handlerType.MakeGenericType(dtoType);

            services.AddScoped(genericHandlerInterfaceType, genericHandlerType);
        }
    }
    
    private static void AddGetValuesQueryHandlers(IServiceCollection services, IReadOnlyCollection<Type> dtoTypes, Type handlerInterfaceType)
    {
        var queryType = typeof(GetValuesQuery<>);
        var handlerType = typeof(GetValuesQueryHandler<>);

        foreach (var dtoType in dtoTypes)
        {
            var requestType = queryType.MakeGenericType(dtoType);
            var responseType = typeof(ICollection<>).MakeGenericType(dtoType);
            var genericHandlerInterfaceType = handlerInterfaceType.MakeGenericType(requestType, responseType);
            var genericHandlerType = handlerType.MakeGenericType(dtoType);

            services.AddScoped(genericHandlerInterfaceType, genericHandlerType);
        }
    }
    
    private static void AddCreateValueQueryHandlers(IServiceCollection services, IReadOnlyCollection<Type> dtoTypes, Type handlerInterfaceType)
    {
        var queryType = typeof(CreateCommand<>);
        var handlerType = typeof(CreateCommandHandler<>);

        AddChangeHandlersToServices(services, dtoTypes, handlerInterfaceType, queryType, handlerType);
    }
    
    private static void AddUpdateValueQueryHandlers(IServiceCollection services, IReadOnlyCollection<Type> dtoTypes, Type handlerInterfaceType)
    {
        var queryType = typeof(UpdateCommand<>);
        var handlerType = typeof(UpdateCommandHandler<>);

        AddChangeHandlersToServices(services, dtoTypes, handlerInterfaceType, queryType, handlerType);
    }

    private static void AddDeleteValueQueryHandlers(IServiceCollection services, IReadOnlyCollection<Type> dtoTypes, Type handlerInterfaceType)
    {
        var queryType = typeof(DeleteCommand<>);
        var handlerType = typeof(DeleteCommandHandler<>);

        AddChangeHandlersToServices(services, dtoTypes, handlerInterfaceType, queryType, handlerType);
    }

    private static void AddChangeHandlersToServices(
        IServiceCollection services, IReadOnlyCollection<Type> dtoTypes, 
        Type handlerInterfaceType, Type queryType, Type handlerType)
    {
        foreach (var dtoType in dtoTypes)
        {
            var requestType = queryType.MakeGenericType(dtoType);
            var genericHandlerInterfaceType = handlerInterfaceType.MakeGenericType(requestType);
            var genericHandlerType = handlerType.MakeGenericType(dtoType);

            services.AddScoped(genericHandlerInterfaceType, genericHandlerType);
        }
    }
}