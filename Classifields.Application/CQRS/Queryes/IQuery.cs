namespace Classifields.Application.CQRS.Queryes;

internal interface IQuery<out TResponse> : IRequest<TResponse>
{

}
