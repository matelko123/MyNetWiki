using Custom.Core;
using Shared.Repositories;

namespace Custom.Features.Users.Commands;

internal sealed class DeleteUserCommandHandler(IUserRepository repo) : ICommandHandler<DeleteUserCommand>
{
    public Task HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        repo.Delete(command.Id);
        return Task.CompletedTask;
    }
}