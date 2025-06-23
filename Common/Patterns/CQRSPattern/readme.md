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

## CQRS in .NET

In .NET, CQRS is often implemented using libraries like [MediatR](https://github.com/jbogard/MediatR) to dispatch commands and queries to their handlers, promoting clean separation and testability.

## Example: User Feature with Commands and Handlers

Below is a simplified example of how to implement a command and its handler using MediatR for a user creation scenario.

### Command

A command represents an intention to perform an action that changes the system state. For example, creating a user:

```csharp
// Features/Users/Commands/CreateUserCommand.cs
using MediatR;

namespace MediatR_Example.Features.Users.Commands;

public record CreateUserCommand(string Name) : IRequest<Guid>;
```

### Command Handler

The handler processes the command. It contains the business logic for the operation:

```csharp
// Features/Users/Commands/CreateUserCommandHandler.cs
using MediatR_Example.Features.Users.Repositories;
using MediatR;

namespace MediatR_Example.Features.Users.Commands;

public class CreateUserCommandHandler(IUserRepository repo) : IRequestHandler<CreateUserCommand, Guid>
{
    public Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        => Task.FromResult(repo.Create(request.Name));
}
```

### Registering and Using Commands

Commands and handlers are registered in the DI container. In a minimal API, you can use MediatR to send commands:

```csharp
// Program.cs (excerpt)
app.MapPost("/users", async (CreateUserCommand cmd, IMediator mediator) =>
{
    var id = await mediator.Send(cmd);
    return Results.Created($"/users/{id}", new { Id = id });
});
```

## Summary

- **Commands** encapsulate write operations.
- **Handlers** process commands and queries.
- **MediatR** decouples the API from the business logic.
- **CQRS** enables clear separation of concerns, scalability, and maintainability.

---

This repository demonstrates a basic CQRS setup using MediatR, with minimal API endpoints for user management. See the `Features/Users/Commands` folder for more command examples.
