using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using VNTrackPal.Application.Persistence;
using VNTrackPal.Persistence.Interceptors;
using Microsoft.Extensions.Configuration;
using VNTrackPal.Persistence.Repositories;
using VNTrackPal.Persistence.DependencyInjection.Options;

namespace VNTrackPal.Persistence.DependencyInjection.Extesions;

public static class ServiceCollectionExtensions
{
    public static void AddSqlConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        // Regist InitialData
        services.AddSingleton<InitialData>();

        services.AddDbContextPool<DbContext, ApplicationDbContext>((provider, builder) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();

            builder
            .EnableDetailedErrors(true)
            .EnableSensitiveDataLogging(false)
            .UseLazyLoadingProxies(false) // => If UseLazyLoadingProxies, all of the navigation fields should be VIRTUAL
            .UseSqlServer(
                connectionString: configuration.GetConnectionString("Database"),
                    sqlServerOptionsAction: optionsBuilder
                        => optionsBuilder
                        .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.GetName().Name));

            var interceptor = provider.GetService<ISaveChangesInterceptor>();
            if (interceptor is not null)
            {
                builder.AddInterceptors(interceptor);
            }
        });
    }

    public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>))
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}

