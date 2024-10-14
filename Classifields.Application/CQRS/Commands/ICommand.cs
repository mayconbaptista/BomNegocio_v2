namespace Classifields.Application.CQRS.Commands;

internal interface ICommand : IRequest<Unit>
{

}

internal interface ICommand<out TResponse> : IRequest<TResponse>
{

}
