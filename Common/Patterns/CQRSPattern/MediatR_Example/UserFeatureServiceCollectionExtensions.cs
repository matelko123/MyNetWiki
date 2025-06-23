using MediatR_Example.Features.Users.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace MediatR_Example;

public static class UserFeatureServiceCollectionExtensions
{
    public static IServiceCollection AddUserFeatures(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateUserCommand>());
        return services;
    }
}