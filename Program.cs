using Microsoft.EntityFrameworkCore;
using workstation_backend.Shared.Infrastructure.Persistence.Configuration;
using workstation_backend.Shared.Infrastructure.Persistence.Repositories;
using FluentValidation;

using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Infrastructure;
using workstation_backend.Shared.Domain.Repositories;
using workstation_backend.OfficesContext.Domain.Services;
using workstation_backend.OfficesContext.Application.QueryServices;
using workstation_backend.OfficesContext.Application.CommandServices;
using workstation_backend.OfficesContext.Domain.Models.Validator;

using workstation_backend.UserContext.Domain;
using workstation_backend.UserContext.Infrastructure;
using workstation_backend.UserContext.Domain.Services;
using workstation_backend.UserContext.Application.QueryServices;
using workstation_backend.UserContext.Application.CommandServices;
using workstation_backend.UserContext.Domain.Models.Validators;

using Microsoft.OpenApi.Models;
using System.Reflection;
using workstation_backend.UserContext.Application.HashServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Workstation API",
        Version = "v1",
        Description = "API para gestión de estaciones de trabajo",
        Contact = new OpenApiContact
        {
            Name = "Tu Nombre",
            Email = "tu-email@example.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
    throw new Exception("Database connection string is not set.");

builder.Services.AddDbContext<WorkstationContext>(options =>
{
    options.UseMySQL(connectionString)
           .LogTo(Console.WriteLine, LogLevel.Error)
           .EnableDetailedErrors();
});

// Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IOfficeRepository, OfficeRepository>();
builder.Services.AddScoped<IOfficeQueryService, OfficeQueryService>();
builder.Services.AddScoped<IOfficeCommandService, OfficeCommandService>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<RatingCommandService>();
builder.Services.AddValidatorsFromAssemblyContaining<OfficeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<OfficeServiceValidator>();

builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IJwtEncryptService, JwtEncryptService>();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentCorsPolicy", policy =>
    {
        policy.WithOrigins("https://frontend-workstation.web.app/dashboard")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


var app = builder.Build();

app.UseCors("DevelopmentCorsPolicy");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workstation API V1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WorkstationContext>();
    context.Database.EnsureCreated();
}

app.Run();
