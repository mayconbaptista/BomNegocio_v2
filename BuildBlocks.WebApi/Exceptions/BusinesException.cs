using System.Net;

namespace BuildBlocks.WebApi.Exceptions
{
    public class BusinesException : Exception
    {
        public HttpStatusCode Code { get; init; }
        public List<string>? erros;

        public BusinesException(HttpStatusCode statusCode, string message, List<string>? erros = null) : base(message)
        {
            Code = statusCode;
            this.erros = erros;
        }
    }
}
