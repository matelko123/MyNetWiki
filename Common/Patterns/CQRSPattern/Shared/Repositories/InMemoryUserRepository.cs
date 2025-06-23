using Shared.Models;

namespace Shared.Repositories;

internal sealed class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<Guid, User> _users = new();

    public User? GetById(Guid id) => _users.GetValueOrDefault(id);

    public User? GetByName(string name) =>
        _users.Values.FirstOrDefault(x => x.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

    public Guid Create(string name)
    {
        Guid id = Guid.NewGuid();
        _users[id] = new User { Id = id, Name = name };
        return id;
    }

    public void Delete(Guid id) => _users.Remove(id);
}