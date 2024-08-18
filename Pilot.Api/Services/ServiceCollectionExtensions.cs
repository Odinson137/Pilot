using System.Reflection;
using MediatR;
using Pilot.Api.Handlers;
using Pilot.Contracts.Base;
using Pilot.SqrsControllerLibrary.Commands;

namespace Pilot.Api.Services;

public static class ServiceCollectionExtensions
{
    public static void AddQueryHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaceType = typeof(IRequestHandler<,>);
        var queryType = typeof(GetValuesQuery<>);
        var handlerType = typeof(GetValuesQueryHandler<>);

        var dtoTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(BaseDto).IsAssignableFrom(t))
            .ToList();

        foreach (var dtoType in dtoTypes)
        {
            var requestType = queryType.MakeGenericType(dtoType);
            var responseType = typeof(ICollection<>).MakeGenericType(dtoType);
            var genericHandlerInterfaceType = handlerInterfaceType.MakeGenericType(requestType, responseType);
            var genericHandlerType = handlerType.MakeGenericType(dtoType);

            services.AddScoped(genericHandlerInterfaceType, genericHandlerType);
        }
    }
}