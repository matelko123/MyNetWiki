using ResultPattern;
using ResultPattern.Api.Entities;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () =>
{
    Result<Guid> result = UserServiceTests.CreateNewUser();
    return result
        ? Results.Ok(result.Value)
        : result.Error; // It will be translated into IResult
}).WithDescription("It will return new Guid every time with status 200 OK");

app.MapGet("/invalid-user", () =>
{
    (User? user, Error? error) = UserServiceTests.CreateInvalidUser();
    if (error is not null)
    {
        return Results.BadRequest(error.Description);
    }
    
    return Results.Ok(user);
}).WithDescription($"It will throw an error {nameof(User.Errors.UserAlreadyExist)}");

app.MapGet("/email-required", () =>
{
    Result user = UserServiceTests.EmailIsRequired();
    return user ? Results.Ok(user) : user.Error;
}).WithDescription($"It will throw an error {nameof(User.Errors.EmailIsRequired)}");

app.Run();


internal static class UserServiceTests
{
    public static Result<Guid> CreateNewUser() => Result<Guid>.Success(Guid.NewGuid());
    public static Result<User> CreateInvalidUser() => Result<User>.Failure(User.Errors.UserAlreadyExist);
    public static Result EmailIsRequired() => Result.Failure(User.Errors.EmailIsRequired);
}