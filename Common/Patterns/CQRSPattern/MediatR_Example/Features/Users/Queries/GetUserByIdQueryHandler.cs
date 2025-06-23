using MediatR_Example.Features.Users.Models;
using MediatR_Example.Features.Users.Repositories;
using MediatR;

namespace MediatR_Example.Features.Users.Queries;

internal sealed class GetUserByIdQueryHandler(IUserRepository repo) : IRequestHandler<GetUserByIdQuery, User?>
{
    public Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        => Task.FromResult(repo.GetById(request.Id));
}