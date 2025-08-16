using Microsoft.EntityFrameworkCore;
using TeambaseInsurance.Extensions;
using TeambaseInsurance.Presentation.ActionFilters;
using TeambaseInsurance.Service.Mapping;
using TeambaseInsurance.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<EmployeeMappingProfile>();
});

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServices();

builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddControllers().AddApplicationPart(typeof(TeambaseInsurance.Presentation.AssemblyReference).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Test database connection and apply migrations
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        // Test database connection
        logger.LogInformation("Testing database connection...");
        var canConnect = context.Database.CanConnect();
        if (canConnect)
        {
            logger.LogInformation("Database connection successful.");
        }
        else
        {
            logger.LogWarning("Database connection failed.");
        }
        
        // Apply migrations
        logger.LogInformation("Starting database migration...");
        context.Database.Migrate();
        logger.LogInformation("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogWarning("Migration failed, attempting to create database: {Message}", ex.Message);
        try
        {
            context.Database.EnsureCreated();
            logger.LogInformation("Database created successfully.");
        }
        catch (Exception createEx)
        {
            logger.LogError(createEx, "Failed to create database: {Message}", createEx.Message);
            throw;
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
