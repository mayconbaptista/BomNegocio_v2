using MediatR;

namespace BuildInBlocks.CQRS.Abstractions
{
    public interface IQuery<out TResult> 
        : IRequest<TResult> where TResult : notnull
    {

    }
}
