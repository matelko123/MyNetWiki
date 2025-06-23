using Custom.Core;
using Shared.Models;
using Shared.Repositories;

namespace Custom.Features.Users.Queries;

internal sealed class GetUserByNameQueryHandler(IUserRepository repo) : IQueryHandler<GetUserByNameQuery, User?>
{
    public Task<User?> HandleAsync(GetUserByNameQuery query, CancellationToken cancellationToken = default)
        => Task.FromResult(repo.GetByName(query.Name));
}