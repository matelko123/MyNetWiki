using MediatR_Example.Features.Users.Models;
using MediatR;

namespace MediatR_Example.Features.Users.Queries;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<User?>;