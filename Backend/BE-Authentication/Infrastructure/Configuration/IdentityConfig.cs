using Domain.Entities;
using Infra.Data.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Services.Settings;
using System.Text;

namespace Infrastructure.Configuration;

public static class IdentityConfig
{
    private const string JWT_CONFIG = "JwtBearerTokenSettings";

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        TokenSettings(services, configuration);

        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequiredLength = 2;
            options.Lockout.MaxFailedAccessAttempts = 6;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddJwtSecurity(configuration);
        services.AddScoped<JwtSecurityExtensionEvents>();

        return services;
    }

    private static void TokenSettings(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettingsSection = configuration.GetSection(JWT_CONFIG);
        var jwtSettings = jwtSettingsSection.Get<JwtSettings>() ?? throw new ArgumentNullException(nameof(configuration), "JWT settings cannot be null");
        services.AddSingleton(jwtSettings);
    }

    public static IServiceCollection AddJwtSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettingsSection = configuration.GetSection(JWT_CONFIG);
        var key = Encoding.UTF8.GetBytes(
            configuration["JwtBearerTokenSettings:SecretKey"]
            ?? throw new ArgumentNullException(
                nameof(configuration), 
                "JwtBearerTokenSettings:SecretKey is null"));

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddCookie(x => { x.Cookie.Name = "token"; })
        .AddJwtBearer(opt =>
        {
            opt.RequireHttpsMetadata = true;
            opt.SaveToken = true;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = configuration["JwtBearerTokenSettings:Audience"],

                ValidateIssuer = true,
                ValidIssuer = configuration["JwtBearerTokenSettings:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateLifetime = true,
                RequireExpirationTime = true,

                ClockSkew = TimeSpan.Zero
            };
            opt.EventsType = typeof(JwtSecurityExtensionEvents);
            opt.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["access_token"];
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization(auth =>
        {
            auth.AddPolicy(
                "Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                .RequireAuthenticatedUser().Build());
        });

        return services;
    }
}
