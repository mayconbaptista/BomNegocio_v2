
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BuildBlocks.WebApi.Exceptions.Handlers
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, exception.Message, DateTime.UtcNow);



            (string detail, string title, int statusCode)  details = exception switch
            {
                BusinesException businessException => 
                    (
                        businessException.Message,
                        businessException.GetType().Name, 
                        (int)businessException.Code
                    ),
                ValidationException validationException => 
                    (
                        validationException.Message,
                        validationException.GetType().Name,
                        StatusCodes.Status422UnprocessableEntity
                    ),
                _ => 
                    (
                        exception.Message, 
                        exception.GetType().Name,
                        StatusCodes.Status500InternalServerError
                    )
            };

            var problemDetails = new ProblemDetails
            {
                Title = details.title,
                Detail = details.detail,
                Status = details.statusCode,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            var jsonProperties = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, jsonProperties), cancellationToken: cancellationToken);

            return true;
        }
    }
}
