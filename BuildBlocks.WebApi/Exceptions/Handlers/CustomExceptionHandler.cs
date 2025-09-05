using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BuildBlocks.WebApi.Exceptions.Handlers
{
    public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            (string detail, string title, int statusCode)  details = exception switch
            {
                System.ComponentModel.DataAnnotations.ValidationException validationException => 
                    (
                        validationException.Message,
                        validationException.GetType().Name,
                        StatusCodes.Status422UnprocessableEntity
                    ),
                FluentValidation.ValidationException fluentValidationException =>
                (
                    fluentValidationException.Message,
                    fluentValidationException.GetType().Name,
                    StatusCodes.Status422UnprocessableEntity
                ),
                BadHttpRequestException badHtpReqException =>
                    (
                        badHtpReqException.Message,
                        badHtpReqException.GetType().Name,
                        StatusCodes.Status400BadRequest
                    ),
                NotFoundException notFoundException =>
                    (
                        notFoundException.Message,
                        notFoundException.GetType().Name,
                        StatusCodes.Status404NotFound
                    ),
                _ => 
                    (
                        exception.Message, 
                        exception.GetType().Name,
                        StatusCodes.Status500InternalServerError
                    )
            };

            if(details.statusCode != StatusCodes.Status500InternalServerError)
            {
                logger.LogWarning(exception, exception.Message);
            }
            else
            {
                exception.Data["HttpContext"] = httpContext.ToString();

                logger.LogError(exception, exception.Message);
            }

            var problemDetails = new ProblemDetails
            {
                Title = details.title,
                Detail = details.detail,
                Status = details.statusCode,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

            httpContext.Response.StatusCode = problemDetails.Status.Value;

            if (exception is System.ComponentModel.DataAnnotations.ValidationException dataAnnotationValidationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", dataAnnotationValidationException.ValidationResult);
            }
            else if(exception is FluentValidation.ValidationException domainValidationException)
            {
                problemDetails.Extensions.Add("ValidationErrors", domainValidationException.Errors);
            }

            var jsonProperties = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(problemDetails, jsonProperties), cancellationToken: cancellationToken);

            return true;
        }
    }
}
