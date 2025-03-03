using Infra.CrossCutting.IoC;
using Microsoft.OpenApi.Models;
using Infra.CrossCutting.IoC.Configuration;

var builder = WebApplication.CreateBuilder(args);
var apiVersion = builder.Configuration.GetValue<string>("ApiVersion");

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDataProtection();
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddAppDbConnections(builder.Configuration);
builder.Services.AddAppDependencyInjection(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));

#region Swagger Documentation
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
                    "This is a system built for encrypt/decrypt passwords and files, generate passwords, store passwords/sensitive data\r\n\r\n" +
                    "Type 'Bearer' + your token in the input below, ex.: 'Bearer XYZ'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
    });
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Akira Digital Solutions - PassAuthKeeper API",
        Version = "v1",
        Description = "This is a system built for encrypt/decrypt passwords and files, generate passwords, store passwords/sensitive data.",
        Contact = new OpenApiContact
        {
            Name = "Mateus Henrique",
            Email = "akiradigitalsolutionss@gmail.com",
            Url = new Uri("https://akiradigitalsolutions.com")
        },
        License = new OpenApiLicense
        {
            Name = "Copyright 2025 Akira Digital Solutions",
            Url = new Uri("https://github.com/AkiraMatthew/ADS.PassAuthKeeper?tab=License-1-ov-file")
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

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.UseRouting();
app.Run();