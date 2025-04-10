using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddAppDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositoriesDependencyInjection(services);
        AddServicesDependencyInjection(services);

        return services;
    }

    public static void AddRepositoriesDependencyInjection(IServiceCollection services)
    {

    }

    public static void AddServicesDependencyInjection(IServiceCollection services)
    {

    }
}
