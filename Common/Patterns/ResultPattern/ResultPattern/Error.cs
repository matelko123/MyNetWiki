using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ResultPattern;

public record Error(HttpStatusCode Code, string Description) : IResult
{
    public static implicit operator bool(Error? error) => error is not null;
    
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = (int)Code;
        httpContext.Response.ContentType = "application/json";

        var problemDetails = new ProblemDetails
        {
            Status = (int)Code,
            Title = Description
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}