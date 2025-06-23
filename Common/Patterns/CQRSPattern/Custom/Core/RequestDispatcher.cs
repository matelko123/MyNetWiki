using Microsoft.Extensions.DependencyInjection;

namespace Custom.Core;

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

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        using var scope = serviceProvider.CreateScope();
        Type handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        dynamic handler = scope.ServiceProvider.GetService(handlerType)
                          ?? throw new InvalidOperationException($"Handler for {request.GetType().Name} not registered.");
        return await handler.HandleAsync((dynamic)request, ct);
    }
}