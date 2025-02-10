using VNTrackPal.Infrastructure.Services;

namespace VNTrackPal.Infrastructure.DepedencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddConfigurationRedis
       (this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>
            (_ => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        services.AddSingleton<IResponseCacheService, ResponseCacheService>();
    }


    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPasswordHashService, PasswordHashService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<ITokenGeneratorService, TokenGeneratorService>()
                .AddScoped<IMediaService, MediaService>();

        return services;
    }
}