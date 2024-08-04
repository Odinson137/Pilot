using Microsoft.Extensions.DependencyInjection;
using Pilot.Contracts.Interfaces;

namespace Pilot.Contracts.Services;

public static class AddingUserService
{
    public static void AddUserService(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}