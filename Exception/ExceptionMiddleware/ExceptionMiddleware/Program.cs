using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using ExceptionMiddleware.Middlewares;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Use the Problem Details format for (empty) non-successful responses
builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance =
            $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

        Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
        context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
    };
});

// Register exception handler
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


// Return the body of the response when the status code is not successful
// The default behavior is to return an empty body with a Status Code
app.UseStatusCodePages();

// Converts unhandled exceptions into Problem Details responses
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    // .NET 9 introduces a simpler way to map exceptions to status codes. Great news for fans of throwing exceptions. 
    StatusCodeSelector = ex => ex switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        UserNotFoundException => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    }
});

// Use detailed info for developers only in development environment
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", (int days) =>
{
    if (days < 1)
    {
        throw new ValidationException("Days must be at least 1.");
    }

    var forecast = Enumerable.Range(1, days).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
        .ToArray();
    return forecast;
});

app.MapGet("/exception", () => { throw new Exception("Oops... something went wrong."); });

app.MapGet("/user-not-found-exception", () => { throw new UserNotFoundException(); });

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class UserNotFoundException() : Exception("User not found.");