using MediatR;
using Shared.Models;

namespace MediatR_Example.Features.Users.Queries;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<User?>;