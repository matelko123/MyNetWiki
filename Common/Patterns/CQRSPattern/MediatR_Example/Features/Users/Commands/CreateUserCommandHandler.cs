using MediatR;
using Shared.Repositories;

namespace MediatR_Example.Features.Users.Commands;

internal sealed class CreateUserCommandHandler(IUserRepository repo) : IRequestHandler<CreateUserCommand, Guid>
{
    public Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        => Task.FromResult(repo.Create(request.Name));
}