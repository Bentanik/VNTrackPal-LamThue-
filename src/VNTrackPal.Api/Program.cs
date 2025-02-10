using HealthChecks.UI.Client;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using VNTrackPal.Api.DepedencyInjection.Extensions;
using VNTrackPal.Application.DependencyInjection.Extensions;
using VNTrackPal.Contract.Exceptions.Middleware;
using VNTrackPal.Infrastructure.DepedencyInjection.Extensions;
using VNTrackPal.Persistence.DependencyInjection.Extesions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllers()
    .AddApplicationPart(VNTrackPal.Presentation.AssemblyReference.Assembly);

// Configure appsettings
builder.Services.AddConfigurationAppSetting(builder.Configuration);

// Configure Authentication
builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

// Regist MediatR
builder.Services.AddConfigureMediatR();

// Regist Middlewares
builder.Services.AddScoped<ExceptionHandlingMiddleware>();

// Regist SQL
builder.Services.AddSqlConfiguration();

// Regist Redis
builder.Services.AddConfigurationRedis(builder.Configuration);

// Regist HealthChecks
builder.Services.AddHealthChecks()
     .AddSqlServer(builder.Configuration.GetConnectionString("Database")!)
     .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

// Register Persistence services
builder.Services.AddPersistenceServices();

// Register Infrastructure service
builder.Services.AddInfrastructureServices(builder.Configuration);

// Configure Swagger
builder.Services
        .AddSwaggerGenNewtonsoftSupport()
        .AddFluentValidationRulesToSwagger()
        .AddEndpointsApiExplorer()
        .AddSwagger();

// Register version api
builder.Services
    .AddApiVersioning(options => options.ReportApiVersions = true)
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// Configure the HTTP request pipeline.
if (builder.Environment.IsDevelopment() || builder.Environment.IsStaging())
    app.ConfigureSwagger();

// Use Exception Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Seed initial data Database
await app.InitialiseDatabaseAsync();

// CORS
app.UseCors("AllowAll");

try
{
    await app.RunAsync();
}
catch (Exception ex)
{
    await app.StopAsync();
}
finally
{
    await app.DisposeAsync();
}

public partial class Program { }
