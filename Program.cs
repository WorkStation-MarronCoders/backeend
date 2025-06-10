using Microsoft.EntityFrameworkCore;
using workstation_backend.Shared.Infrastructure.Persistence.Configuration;
using workstation_backend.Shared.Infrastructure.Persistence.Repositories;
using workstation_backend.Shared.Domain;
using workstation_backend.OfficesContext.Domain;
using workstation_backend.OfficesContext.Infrastructure;
using workstation_backend.Shared.Domain.Repositories;
using workstation_backend.OfficesContext.Domain.Services;
using workstation_backend.OfficesContext.Application.QueryServices;
using workstation_backend.OfficesContext.Application.CommandServices;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

// News Bounded Context Injection Configuration
builder.WebHost.UseUrls("http://localhost:5000");


var app = builder.Build();

// Verify Database Objects are created
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WorkstationContext>();
    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();