using MediatR;

namespace BuildBlocks.Domain.Abstractions.CQRS
{
    public interface IQuery<out TResult>
        : IRequest<TResult> where TResult : notnull
    {

    }
}
