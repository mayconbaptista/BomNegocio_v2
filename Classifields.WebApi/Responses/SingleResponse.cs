namespace Classifields.WebAPI.Responses
{
    public class SingleResponse<TResponse> : DefaultResponse where TResponse : notnull
    {
        public TResponse Data { get; init; }

        public SingleResponse(TResponse data) : base()
        {
            Data = data;
        }

        public SingleResponse(string message) : base(message, null)
        {
        }

        public SingleResponse(string message, string[] errors) : base(message, errors)
        {
        }
    }
}
