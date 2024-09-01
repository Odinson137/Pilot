using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Base;

namespace Pilot.SqrsControllerLibrary.Services;

public static class ServiceRepositoryExtensions
{
    public static void AddBaseRepositories(this IServiceCollection services, Assembly assembly)
    {
        var modelTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(BaseModel).IsAssignableFrom(t))
            .ToList();
        
        var interfaceType = typeof(IBaseRepository<>);
        var repositoryType = typeof(BaseRepository<>);

        AddRepository(services, modelTypes, interfaceType, repositoryType);
    }

    private static void AddRepository(IServiceCollection services, IReadOnlyCollection<Type> modelTypes, Type interfaceType, Type repositoryType)
    {
        foreach (var modelType in modelTypes)
        {
            var genericInterfaceType = interfaceType.MakeGenericType(modelType);
            var genericRealizationType = repositoryType.MakeGenericType(modelType);

            services.AddScoped(genericInterfaceType, genericRealizationType);
        }
    }
}