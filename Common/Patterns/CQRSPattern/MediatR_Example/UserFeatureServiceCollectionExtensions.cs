using MediatR_Example.Features.Users.Commands;
using MediatR_Example.Features.Users.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR_Example;

public static class UserFeatureServiceCollectionExtensions
{
    public static IServiceCollection AddUserFeatures(this IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommand>());
        return services;
    }
}