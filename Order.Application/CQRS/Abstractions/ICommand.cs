
using MediatR;

namespace Order.Application.CQRS.Abstractions;

public interface ICommand : ICommand<Unit>
{

}

public interface ICommand<out TResult> : IRequest<TResult>
{

}
