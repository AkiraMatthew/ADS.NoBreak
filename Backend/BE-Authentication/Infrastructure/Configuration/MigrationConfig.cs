﻿using Infra.Data.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration;

public static class MigrationConfig
{
    public static void RunMigrations(this WebApplication app)
    {
        using var serviceScope = app.Services.CreateScope();

        using var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        if (context != null)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }
    }
}
