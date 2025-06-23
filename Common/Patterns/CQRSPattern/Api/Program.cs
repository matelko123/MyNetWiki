using Custom;
using Custom.Core;
using Custom.Features.Users.Commands;
using Custom.Features.Users.Queries;
using MediatR_Example;
using MediatR_Example.Features.Users.Commands;
using MediatR_Example.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRepositories();

// Register MediatR and user features
builder.Services.AddUserFeatures();

// Register custom CQRS
builder.Services.AddRequestHandlers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Minimal API endpoints for User features
// MediatR
app.MapPost("/users", async (CreateUserCommand cmd, ISender sender) =>
{
    var id = await sender.Send(cmd);
    return Results.Created($"/users/{id}", new { Id = id });
});

app.MapGet("/users/{id:guid}", async (Guid id, ISender sender) =>
{
    var user = await sender.Send(new GetUserByIdQuery(id));
    return user is not null ? Results.Ok(user) : Results.NotFound();
});

// Custom
app.MapGet("/users/by-name/{name}", async (string name, [FromServices] IRequestDispatcher dispatcher) =>
{
    var user = await dispatcher.SendAsync(new GetUserByNameQuery(name));
    return user is not null ? Results.Ok(user) : Results.NotFound();
});
app.MapDelete("/users/{id:guid}", async (Guid id, [FromServices] Custom.Core.IRequestHandler<DeleteUserCommand> handler) =>
{
    await handler.HandleAsync(new DeleteUserCommand(id));
    return Results.NoContent();
});

app.Run();
