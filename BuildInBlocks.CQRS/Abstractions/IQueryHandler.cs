using MediatR;

namespace BuildInBlocks.CQRS.Abstractions
{
    public interface IQueryHandler<in TQuery, TResult>
        : IRequestHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
        where TResult : notnull
    {
    }
}
