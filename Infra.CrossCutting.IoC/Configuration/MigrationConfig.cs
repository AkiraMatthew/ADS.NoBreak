using Infra.Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC;

public static class MigrationConfig
{
    public static void RunMigrations(this WebApplication app)
    {
        using IServiceScope serviceScope = app.Services.CreateScope();
        using LoginContext context = serviceScope.ServiceProvider.GetService<LoginContext>()!;
        context?.Database.Migrate();
    }
}
