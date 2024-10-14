using System.Collections.Generic;

namespace Classifields.WebAPI.Responses
{
    public class ListResponse<TResponse> where TResponse : class
    {
        public ICollection<TResponse> Data { get; init; }

        public ListResponse(ICollection<TResponse> data) : base()
        {
            Data = data;
        }

        public ListResponse(string message) : base(message, null)
        {
            Data = null;
        }

        public ListResponse(string message, string[] errors) : base(message, errors)
        {
            Data = null;
        }
    }
}
