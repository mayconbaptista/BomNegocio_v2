using MediatR;

namespace BuildBlocks.WebApi.Behaviors
{
    public class ExceptionHandlingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                var response = await next();

                return response;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw;
            }
        }
    }
}
