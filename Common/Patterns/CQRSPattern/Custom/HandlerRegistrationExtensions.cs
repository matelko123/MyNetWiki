using Custom.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Custom;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IRequestDispatcher, RequestDispatcher>();
        
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(DependencyInjectionExtensions))
            .AddClasses(classes => classes.AssignableToAny(
                typeof(IRequestHandler<>),
                typeof(IRequestHandler<,>)
            ), false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        return services;
    }
}