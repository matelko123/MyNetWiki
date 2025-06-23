# CQRS Pattern in .NET

## What is CQRS?

**CQRS (Command Query Responsibility Segregation)** is a software architectural pattern that separates read and write operations into different models. Instead of using the same model for querying and updating data, CQRS uses distinct models for each, improving scalability, maintainability, and performance.

## Key Concepts

- **Command**: An operation that changes state (e.g., CreateUser, UpdateOrder).
- **Query**: An operation that reads state (e.g., GetUserById, ListOrders).
- **Handler**: Processes a command or query.
- **Separation**: Commands and queries are handled by different objects, often with different data models.

## Benefits

- **Scalability**: Read and write workloads can be scaled independently.
- **Simplicity**: Each operation is focused and easier to reason about.
- **Security**: Write operations can be more tightly controlled.

---

## CQRS in .NET – Two Approaches

This repository demonstrates two ways to implement CQRS in .NET:

### 1. Using MediatR

MediatR is a popular library for in-process messaging, often used to implement CQRS by dispatching commands and queries to their handlers.

#### Example: User Feature with Commands and Handlers

```csharp
// Features/Users/Commands/CreateUserCommand.cs
using MediatR;

namespace MediatR_Example.Features.Users.Commands;

public sealed record CreateUserCommand(string Name) : IRequest<Guid>;
```

```csharp
// Features/Users/Commands/CreateUserCommandHandler.cs
using MediatR_Example.Features.Users.Repositories;
using MediatR;

namespace MediatR_Example.Features.Users.Commands;

internal sealed class CreateUserCommandHandler(IUserRepository repo) : IRequestHandler<CreateUserCommand, Guid>
{
    public Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        => Task.FromResult(repo.Create(request.Name));
}
```

**Usage in Minimal API:**
```csharp
app.MapPost("/users", async (CreateUserCommand cmd, IMediator mediator) =>
{
    var id = await mediator.Send(cmd);
    return Results.Created($"/users/{id}", new { Id = id });
});
```

> **Note:** MediatR is no longer actively maintained. For new projects, consider a custom approach.

---

### 2. Custom CQRS Implementation

A custom implementation gives you full control, removes external dependencies, and is future-proof. Below is a simplified example based on the `Custom` project in this repository.

#### Core Interfaces

```csharp
// Marker for all requests
public interface IRequest { }

// Command and Query markers
public interface ICommand : IRequest {}
public interface ICommand<TResult> : IRequest {}
public interface IQuery : IRequest {}
public interface IQuery<TResult> : IRequest {}

// Handler interfaces
public interface IRequestHandler<in TRequest, TResult>
    where TRequest : class, IRequest
{
    Task<TResult> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
public interface IRequestHandler<in TRequest>
    where TRequest : class, IRequest
{
    Task HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}
```

#### Dispatcher Example

```csharp
public class RequestDispatcher(IServiceProvider serviceProvider) : IRequestDispatcher
{
    public async Task SendAsync(IRequest request, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        Type handlerType = typeof(IRequestHandler<>).MakeGenericType(request.GetType());
        dynamic handler = scope.ServiceProvider.GetService(handlerType)
                          ?? throw new InvalidOperationException($"Handler for {request.GetType().Name} not registered.");
        await handler.HandleAsync((dynamic)request, ct);
    }
    ...
}
```

#### DI Registration

```csharp
public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        services.AddSingleton<IRequestDispatcher, RequestDispatcher>();
        
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(DependencyInjectionExtensions))
            .AddClasses(classes => classes.AssignableToAny(
                typeof(IRequestHandler<>),
                typeof(IRequestHandler<,>)
            ), false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        return services;
    }
}
```

---

## Recommendation

While MediatR is widely used and easy to integrate, its end of support means you should consider a custom CQRS implementation for new projects. The custom approach shown here is simple, flexible, and avoids external dependencies, making it a robust choice for future-proof .NET solutions.

---

This repository demonstrates both approaches. For new projects, **we recommend the custom CQRS implementation**.
