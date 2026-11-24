
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;

namespace Order.Infrastructure.Payment.Handlers
{
    internal class AuditDelegatingHandler(ILogger<AuditDelegatingHandler> logger) : DelegatingHandler
    {
        private readonly ILogger<AuditDelegatingHandler> _logger = logger;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this._logger.LogInformation("RequestJson: {request}", request);

            var response = await base.SendAsync(request, cancellationToken);

            this._logger.LogInformation("ResponseJson: {response}", response);

            return response;
        }
    }
}
