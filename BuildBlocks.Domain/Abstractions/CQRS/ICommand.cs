using MediatR;

namespace BuildBlocks.Domain.Abstractions.CQRS;

public interface ICommand : ICommand<Unit>
{

}

public interface ICommand<out TResult> : IRequest<TResult>
{

}
