namespace Custom.Core;

public interface IRequestHandler<in TRequest> where TRequest : IRequest
{
    Task HandleAsync(TRequest request, CancellationToken ct = default);
}

public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken ct = default);
}