using Custom.Core;

namespace Custom.Features.Users.Commands;

public sealed record DeleteUserCommand(Guid Id) : ICommand;
