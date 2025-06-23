using MediatR_Example;
using MediatR_Example.Features.Users.Commands;
using MediatR_Example.Features.Users.Queries;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register MediatR and user features
builder.Services.AddUserFeatures();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Minimal API endpoints for User features

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

app.Run();
