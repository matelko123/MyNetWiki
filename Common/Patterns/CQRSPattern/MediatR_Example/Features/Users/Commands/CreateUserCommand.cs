using MediatR;

namespace MediatR_Example.Features.Users.Commands;

public sealed record CreateUserCommand(string Name) : IRequest<Guid>;
