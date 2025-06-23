namespace Custom.Core;

public interface IRequest;

public interface IRequest<out TResponse> : IRequest;