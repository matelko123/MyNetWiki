using MediatR_Example.Features.Users.Models;

namespace MediatR_Example.Features.Users.Repositories;

public interface IUserRepository
{
    User? GetById(Guid id);
    Guid Create(string name);
}