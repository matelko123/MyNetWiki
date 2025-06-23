namespace Custom.Core;

public interface IRequestDispatcher
{
    Task SendAsync(IRequest request, CancellationToken ct = default);
    Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken ct = default);
}