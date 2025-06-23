using Shared.Models;

namespace Shared.Repositories;

public interface IUserRepository
{
    User? GetById(Guid id);
    User? GetByName(string name);
    Guid Create(string name);
    void Delete(Guid id);
}