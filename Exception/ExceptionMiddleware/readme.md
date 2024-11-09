# Exception Handling with Problem Details in .NET

This project provides structured exception handling in a .NET API using the **Problem Details** specification to return consistent, machine-readable error information. This setup enhances the API’s usability by offering clear and standardized error responses.

## What we want to achivie?
```json
{
  "type": "Exception",
  "title": "An error occurred",
  "status": 500,
  "detail": "Oops... something went wrong.",
  "instance": "GET /exception",
  "traceId": "00-06b429afce23f67a78bd4d029f450415-d9904fe4ea00150e-00",
  "requestId": "4000003d-0002-fa00-b63f-84710c7967bb"
}
```
Instead of:
```json
System.Exception: Oops... something went wrong.
   at Program.<>c.<<Main>$>b__0_1() in C:\Users\mateu\source\MyNetWiki\Exception\ExceptionMiddleware\ExceptionMiddleware\Program.cs:line 78
   at lambda_method3(Closure, Object, HttpContext)
   at Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware.Invoke(HttpContext context)
   at Swashbuckle.AspNetCore.SwaggerUI.SwaggerUIMiddleware.Invoke(HttpContext httpContext)
   at Swashbuckle.AspNetCore.Swagger.SwaggerMiddleware.Invoke(HttpContext httpContext, ISwaggerProvider swaggerProvider)
   at Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
   at Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)

HEADERS
=======
Accept: application/json
Accept-Encoding: gzip, deflate
Connection: close
Host: localhost:44315
User-Agent: vscode-restclient
```

## Benefits of This Approach
- **Standardized Error Responses**: Provides consistent, structured error data to API clients.
- **Enhanced Debugging**: Includes trace and request IDs in responses to assist with diagnostics.
- **Customizable Status Codes**: Allows mapping of exceptions to appropriate HTTP status codes for clarity.

## Project Components

### 1. `Program.cs` Configuration

The `Program.cs` file configures middleware to handle exceptions and unsuccessful HTTP responses by implementing Problem Details for error messaging.

#### Key Components

- **Problem Details Service**: Adds a custom Problem Details formatter to enrich error responses with:
  - `instance`: Identifies the HTTP method and path.
  - `requestId`: A unique request identifier.
  - `traceId`: Traces the request for debugging.

- **Custom Exception Handler**: Registers `CustomExceptionHandler` to centralize exception handling.
  
- **Status Code Selector**: Maps exceptions to specific HTTP status codes:
  - `ValidationException` → `400 Bad Request`
  - `UserNotFoundException` → `404 Not Found`
  - All other exceptions → `500 Internal Server Error`

#### Example Endpoints

1. **/weatherforecast**: Returns a forecast for a specified number of days.
   - Throws a `ValidationException` if `days` is less than 1.

2. **/exception**: Throws a generic exception for testing error handling.

3. **/user-not-found-exception**: Throws a `UserNotFoundException` to test 404 error handling.

### 2. Custom Exception Handler (`CustomExceptionHandler.cs`)

`CustomExceptionHandler` uses `IProblemDetailsService` to format and respond to exceptions in a structured format. It builds a `ProblemDetails` object containing:

- `Title`: A general description of the error.
- `Type`: The type of exception.
- `Detail`: A message with specifics of the exception.


```csharp
internal sealed class CustomExceptionHandler (IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "An error occurred",
            Type = exception.GetType().Name,
            Detail = exception.Message
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
```

This handler then invokes `ProblemDetailsService.TryWriteAsync()` to write the `ProblemDetails` response, allowing clients to receive detailed, consistent error messages.

## Implementation

### Register Exception Handler:
```csharp
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
```

### Configure handler options:
New: StatusCodeSelector link
With the new addition in **ASP.NET 9**, we can simplify this by using the new StatusCodeSelector configuration property. This addition makes a custom implementation of an exception handler almost unnecessary (for most cases).

```csharp
app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = ex => ex switch
    {
        ValidationException => StatusCodes.Status400BadRequest,
        UserNotFoundException => StatusCodes.Status404NotFound,
        _ => StatusCodes.Status500InternalServerError
    }
});
```

### UseStatusCodePages [Optional]:
Return the body of the response when the status code is not successful.
The default behavior is to return an empty body with a Status Code

```csharp
app.UseStatusCodePages();
```

Response **before** StatusCodePages:
There isn't one. The application set only Response StatusCode.

Response **after** StatusCodePages:
```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404,
  "instance": "GET /endpoint-not-exists",
  "traceId": "00-efc32bedd62bdcaec55fcf68d41ed835-dc7b860b5f7fae69-00",
  "requestId": "40000039-0002-fa00-b63f-84710c7967bb"
}
``` 

### UseDeveloperExceptionPage [Optional]:
Show detailed info for developers only in development environment
```csharp
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
```

## Sources:
### Youtube:
- [The Right Way To Return API Errors in .NET By Nick Chapsas](https://youtu.be/-TGZypSinpw?si=QM_MV9VZrD4TP2PN)
### Blogs:
- [Translating Exceptions into Problem Details Responses By Tim Deschryver](https://timdeschryver.dev/blog/translating-exceptions-into-problem-details-responses#conclusion)
- [Problem Details for ASP.NET Core APIs By Milan Jovanović](https://www.milanjovanovic.tech/blog/problem-details-for-aspnetcore-apis?utm_source=newsletter&utm_medium=email&utm_campaign=tnw112)