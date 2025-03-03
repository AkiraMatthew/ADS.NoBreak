using EntityFrameworkCore.UseRowNumberForPaging;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC.Configuration;

public static class DbConnection
{
    public static IServiceCollection AddAppDbConnections(this IServiceCollection services, IConfiguration configuration)
    {
        var apiConnection = configuration.GetConnectionString("ApiConnection");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(apiConnection,
            providerOptions =>
            {
                providerOptions.CommandTimeout(200);
                providerOptions.UseRowNumberForPaging();
                providerOptions.MigrationsAssembly("Api");
            }));

        return services;
    }
}
