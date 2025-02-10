using VNTrackPal.Application.Behaviors;

namespace VNTrackPal.Application.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConfigureMediatR(this IServiceCollection services)
     => services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyReference.Assembly))
           .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
           .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
           .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>))
           .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true);
}
