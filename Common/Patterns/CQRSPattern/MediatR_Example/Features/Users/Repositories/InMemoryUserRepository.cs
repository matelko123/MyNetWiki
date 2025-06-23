using MediatR_Example.Features.Users.Models;

namespace MediatR_Example.Features.Users.Repositories;

internal sealed class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<Guid, User> _users = new();

    public User? GetById(Guid id) => _users.TryGetValue(id, out var user) ? user : null;

    public Guid Create(string name)
    {
        var id = Guid.NewGuid();
        _users[id] = new User { Id = id, Name = name };
        return id;
    }
}