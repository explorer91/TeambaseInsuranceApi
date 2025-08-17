using Microsoft.EntityFrameworkCore;
using TeambaseInsurance.Repository;
using TeambaseInsurance.RepositoryContracts;
using TeambaseInsurance.Service;
using TeambaseInsurance.ServiceContracts;

namespace TeambaseInsurance.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                        sqlOptions.CommandTimeout(60);
                        sqlOptions.MigrationsAssembly("TeambaseInsurance");
                    });

                // Enable detailed error messages in development
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IPremiumCalculatorService, PremiumCalculatorService>();
        }

        public static void ApplyMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
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
        }
    }
}
