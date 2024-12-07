
namespace WebApiBlock.Responses
{
    public class ErrorResponse
    {
        public string Message { get; init; }
        public string? StackTrace { get; set; }
        public List<string>? Errors { get; set; }

        public ErrorResponse(string message)
        {
            this.Message = message;
        }
        public ErrorResponse(string message, List<string> errors)
        {
            this.Message = message;
            this.Errors = errors;
        }

        public ErrorResponse(string message, List<string> errors,string stackTrace)
        {
            this.Message = message;
            this.StackTrace = stackTrace;
            this.Errors = errors;
        }

    }
}
