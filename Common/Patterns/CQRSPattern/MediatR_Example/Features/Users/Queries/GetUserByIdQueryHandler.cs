using MediatR;
using Shared.Models;
using Shared.Repositories;

namespace MediatR_Example.Features.Users.Queries;

internal sealed class GetUserByIdQueryHandler(IUserRepository repo) : IRequestHandler<GetUserByIdQuery, User?>
{
    public Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        => Task.FromResult(repo.GetById(request.Id));
}