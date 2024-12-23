using BuildBlocks.WebApi.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;

namespace BuildBlocks.WebApi.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ExceptionMiddleware> _logger = logger;
        private readonly IHostEnvironment _env = env;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, response) = ex switch
            {
                BusinesException buEx => ((int)buEx.Code, new ErrorResponse(buEx.Message)),
                ValidationException vaEx => ((int)HttpStatusCode.UnprocessableEntity, new ErrorResponse(vaEx.Message)),
                _ => ((int)HttpStatusCode.InternalServerError, new ErrorResponse("Erro interno no servidor"))
            };

            context.Response.StatusCode = statusCode;

            var jsonProperties = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonProperties));
        }
    }
}
