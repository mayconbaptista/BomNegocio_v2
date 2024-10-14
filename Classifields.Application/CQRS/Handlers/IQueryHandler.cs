using Classifields.Application.CQRS.Queryes;

namespace Classifields.Application.CQRS.Handlers;

internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
    where TResponse : notnull
{

}