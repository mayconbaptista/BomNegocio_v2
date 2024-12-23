
using MediatR;

namespace Order.Application.CQRS.Abstractions
{
    public interface IQuery<out TResult> 
        : IRequest<TResult>
        where TResult : notnull
    {

    }
}
