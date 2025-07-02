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

// Importar Swagger
using Microsoft.OpenApi.Models;
using System.Reflection;

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
    {
        c.IncludeXmlComments(xmlPath);
    }

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

// Add Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Verify Database Connection String
if (connectionString is null)
    // Stop the application if the connection string is not set.
    throw new Exception("Database connection string is not set.");

// Configure Database Context and Logging Levels
if (builder.Environment.IsDevelopment())
    builder.Services.AddDbContext<WorkstationContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        });
else if (builder.Environment.IsProduction())
    builder.Services.AddDbContext<WorkstationContext>(
        options =>
        {
            options.UseMySQL(connectionString)
                .LogTo(Console.WriteLine, LogLevel.Error)
                .EnableDetailedErrors();
        });

// Shared Bounded Context Injection Configuration
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
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();


builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WorkstationContext>();
    context.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    // Configurar Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workstation API V1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();