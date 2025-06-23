using Microsoft.Extensions.DependencyInjection;

namespace Shared.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddSingleton<IUserRepository, InMemoryUserRepository>();
    }
}