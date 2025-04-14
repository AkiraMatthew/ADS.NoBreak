using EntityFrameworkCore.UseRowNumberForPaging;
using Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration;

public static class DbConnection
{
    public static IServiceCollection AddAppDbConnections(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("connectionString");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString,
            providerOptions =>
            {
                providerOptions.CommandTimeout(200);
                providerOptions.UseRowNumberForPaging();
                providerOptions.MigrationsAssembly("Api");
            }));

        return services;
    }
}
