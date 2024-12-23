namespace BuildBlocks.WebApi.Responses
{
    public class ErrorResponse
    {
        public string Message { get; init; }
        public string? StackTrace { get; set; }
        public List<string>? Errors { get; set; }

        public ErrorResponse(string message)
        {
            Message = message;
        }
        public ErrorResponse(string message, List<string> errors)
        {
            Message = message;
            Errors = errors;
        }

        public ErrorResponse(string message, List<string> errors, string stackTrace)
        {
            Message = message;
            StackTrace = stackTrace;
            Errors = errors;
        }

    }
}
