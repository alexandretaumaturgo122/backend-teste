using MediatR;

namespace Backend.Core.CQRS;

public class Response<T>
{
    public Response(string? errorMessage, bool success, T? data)
    {
        ErrorMessage = errorMessage;
        Success = success;
        Data = data;
    }

    public Response(bool success, T? data) : this(null, success, data) { }

    protected Response() { }

    public string? ErrorMessage { get; protected set; }
    public bool Success { get; protected set; }
    public T? Data { get; protected set; }
}

public sealed record EmptyResponse(bool Success = true)
{
    public static EmptyResponse Instance => new();

    public static Task<EmptyResponse> NewInstance<T>(T _) => Task.FromResult(Instance);
}

public abstract record Command : IRequest<EmptyResponse>;

public abstract class CommandHandler<TCommand> : IRequestHandler<TCommand, EmptyResponse>
    where TCommand : Command
{
    public abstract Task<EmptyResponse> Handle(TCommand request,
        CancellationToken cancellationToken);
} 