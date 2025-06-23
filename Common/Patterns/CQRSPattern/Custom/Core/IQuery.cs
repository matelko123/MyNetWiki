namespace Custom.Core;

public interface IQuery<out TResult> : IRequest<TResult>;