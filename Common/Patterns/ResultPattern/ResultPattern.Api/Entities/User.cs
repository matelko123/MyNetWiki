using System.Net;

namespace ResultPattern.Api.Entities;

public sealed class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;


    public static class Errors
    {
        public static Error UserNotFound => new(HttpStatusCode.NotFound, "User not found");
        public static Error UserAlreadyExist => new(HttpStatusCode.Conflict, "User already exists");
        public static Error EmailIsRequired => new(HttpStatusCode.BadRequest, "Email is required");
        public static Error FirstNameIsRequired => new(HttpStatusCode.BadRequest, "First name is required");
        public static Error LastNameIsRequired => new(HttpStatusCode.BadRequest, "Last name is required");
    }
}