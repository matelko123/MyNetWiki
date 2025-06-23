namespace Shared.Models;

// User entity
public sealed class User
{
    public Guid Id { get; init; }
    public string Name { get; set; } = default!;
}