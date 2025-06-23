using Custom.Core;
using Shared.Models;

namespace Custom.Features.Users.Queries;

public sealed record GetUserByNameQuery(string Name) : IQuery<User?>;