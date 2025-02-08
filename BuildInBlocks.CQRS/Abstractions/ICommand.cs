using MediatR;

namespace BuildInBlocks.CQRS.Abstractions;

public interface ICommand : ICommand<Unit>
{

}

public interface ICommand<out TResult> : IRequest<TResult>
{

}
