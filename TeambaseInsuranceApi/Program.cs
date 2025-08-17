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

ServiceExtensions.ApplyMigrations(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddlewareExtensions>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
