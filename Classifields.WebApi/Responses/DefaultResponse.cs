namespace Classifields.WebAPI.Responses
{
    public class DefaultResponse
    {
        public bool Succeeded { get; init; }
        public string? Message { get; init; }
        public string[]? Errors { get; init; }

        public DefaultResponse()
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
        }

        public DefaultResponse(string message, string[]? errors)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
        }
    }
}
