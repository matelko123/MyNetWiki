using System.Net;

namespace ResultPattern;

public class Result<TValue> : Result
{
    public TValue? Value { get; }

    private Result(TValue? value, Error? error) : base(error) => Value = value;

    public static Result<TValue> Success(TValue? value) => new(value, null);
    public new static Result<TValue> Failure(Error error) => new(default, error);
    public new static Result<TValue> Failure(HttpStatusCode code, string description) => Failure(new Error(code, description));
    public new static Result<TValue> BadRequest(string description) => Failure(HttpStatusCode.BadRequest, description);
    public new static Result<TValue> NotFound(string description) => Failure(HttpStatusCode.NotFound, description);

    public void Deconstruct(out TValue? value, out Error? error)
    {
        value = Value;
        error = Error;
    }
}