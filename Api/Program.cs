using Domain.Entities;
using Infra.CrossCutting.IoC;
using Infra.Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDataProtection();
#region Versioning
/*builder.Services.AddApiVersioning(
                options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    // reporting api versions will return the headers
                    // "api-supported-versions" and "api-deprecated-versions"
                    options.ReportApiVersions = true;
                    options.ApiVersionReader = ApiVersionReader.Combine(
                           new UrlSegmentApiVersionReader(),
                           new QueryStringApiVersionReader("api-version"),
                           new HeaderApiVersionReader("1-Version"),
                           new MediaTypeApiVersionReader("1-version"));
                })
            .AddMvc(
                options =>
                {
                    // automatically applies an api version based on the name of
                    // the defining controller's namespace
                    options.Conventions.Add(new VersionByNamespaceConvention());
                });

builder.Services.AddDbContext<LoginContext>(options =>
    options.UseSqlServer("ADS-Delivery")
);*/
#endregion

#region IdentityConfig
// Add Identity services with default UI
builder.Services.AddIdentityCore<UserData>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<LoginContext>()
    .AddApiEndpoints();

builder.Services.AddDbContext<LoginContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ApiConnection")
));

#endregion

#region Swagger Documentation
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Akira Digital Solutions - Card�pio Delivery API",
        Version = "v1",
        Description = "This API is about a Delivery Menu registration made by the contracting part. The Contracting Part could be a Restaurant or even a small food delivery.",
        Contact = new OpenApiContact
        {
            Name = "Mateus Henrique",
            Email = "akiradigitalsolutionss@gmail.com",
            Url = new Uri("https://akiradigitalsolutions.com")
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
});
#endregion

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint($"/swagger/v1/swagger.json", "ADS.PassAuthKeeper");
        options.DocumentTitle = "PassAuthKeeper API Documentation";
        options.RoutePrefix = string.Empty;
    });
    app.RunMigrations();
}
/*
app.MapGet("users/me", async (ClaimsPrincipal claims, LoginContext context) =>
{
    string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

    return await context.Users.FindAsync(userId);
})
.RequireAuthorization();*/

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
//app.MapIdentityApi<IdentityUser>();
app.Run();
