# Result Pattern in .NET

The **Result Pattern** is a simple yet powerful approach to handling success and failure without relying on exceptions for control flow. Instead of throwing exceptions, operations return a `Result` object that contains information about the outcome.

This pattern is particularly useful in:

* Domain logic and validation
* Service methods
* APIs where consistent error handling is crucial
* **Unit testing**, where expected errors can be asserted directly from the domain class

## Benefits

* Makes success/failure explicit
* Improves testability and readability
* Avoids exceptions as a flow control mechanism
* Centralizes and reuses error definitions in business logic

---

## Result Implementation

NOTE: Short form for example purposes.

```csharp
public class Result
{
    public Error? Error { get; }
}

public class Result<TValue> : Result
{
    public TValue? Value { get; }
}

public record Error(HttpStatusCode Code, string Description) : IResult;
```

---

## Sample Usage in ASP.NET Core API

```csharp
app.MapGet("/", () =>
{
    Result<Guid> result = UserServiceTests.CreateNewUser();
    return result ? Results.Ok(result.Value) : result.Error;
});

app.MapGet("/invalid-user", () =>
{
    (User? user, Error? error) = UserServiceTests.CreateInvalidUser();
    return error ? Results.BadRequest(error.Description) : Results.Ok(user);
});

app.MapGet("/email-required", () =>
{
    Result user = UserServiceTests.EmailIsRequired();
    return user ? Results.Ok(user) : user.Error;
});
```

---

## Project Structure

```
ResultPattern/
├── ResultPattern.sln
├── ResultPattern/                  # Class Library with Result logic
│   └── Result.cs
├── ResultPattern.Api/              # ASP.NET Core Web API
│   ├── Program.cs
│   ├── Entities/
│   │   └── User.cs
│   └── UserServiceTests.cs
├── ResultPattern.Tests/            # Unit test project
└── README.md                       # You are here
```

---

## Unit Testing Support

One of the strengths of this pattern is how naturally it supports **unit testing**. Because domain errors are defined in the business entity (e.g. `User.Errors.UserAlreadyExist`), tests can assert against specific domain errors directly:

```csharp
[Fact]
public void Should_Return_UserAlreadyExist_Error()
{
    Result<User> result = UserService.CreateInvalidUser();
    result.Error.ShouldBe(User.Errors.UserAlreadyExist);
}
```

This makes tests clean, predictable, and expressive.

---

This approach helps enforce a clear, consistent model for handling success/failure across your .NET codebase. It's a simple tool — but highly effective.