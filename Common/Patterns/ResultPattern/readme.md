# Result Pattern in .NET

The **Result Pattern** is a simple yet powerful approach to handling success and failure without relying on exceptions for control flow. Instead of throwing exceptions, operations return a `Result` object that contains information about the outcome.

This pattern is particularly useful in:

* Domain logic and validation
* Service methods
* APIs where consistent error handling is crucial

## Benefits

* Makes success/failure explicit
* Improves testability and readability
* Avoids exceptions as a flow control mechanism

---

## Result Implementation

NOTE: This is just part of the code for a better visualization of the example

```csharp
public class Result
{
    public Error? Error { get; }
}

public class Result<TValue> : Result
{
    public TValue? Value { get; }
}

public record Error(HttpStatusCode Code, string Description);
```

---

## Sample Usage in ASP.NET Core API

```csharp

app.MapGet("/invalid-user", () =>
{
    (User? user, Error? error) = UserServiceTests.CreateInvalidUser();
    if (error)
    {
        return Results.BadRequest(error.Description);
    }
    
    return Results.Ok(user);
}).WithDescription($"It will throw an error");
```

---

## Project Structure

```
ResultPattern/
├── ResultPattern.sln
├── ResultPattern/             # Class Library with Result logic
│   └── Result.cs
├── ResultPattern.Api/         # ASP.NET Core Web API
│   └── Program.cs
└── README.md                  # You are here
```

You can copy this structure into your GitHub repo to create a reusable pattern. You’ll be able to test it through the API while reusing the library in other apps.

---

This approach helps enforce a clear, consistent model for handling success/failure across your .NET codebase. It's a simple tool — but highly effective.

--- 

## Links

- [What’s the Result Type Everyone Is Using in .NET? ~ Nick Chapsas](https://youtu.be/YbuSuSpzee4?si=IH1tqExIM-mOpSE6)
- [Goodbye Exceptions! Hello Result Pattern! ~ Gui Ferreira](https://youtu.be/C_u1WottRA0?si=3wIbIPLb1B6em2Jv)
- [Get Rid of Exceptions in Your Code With the Result Pattern ~ Milan Jovanović](https://youtu.be/WCCkEe_Hy2Y?si=Hjk04b6_9egUy_eb)