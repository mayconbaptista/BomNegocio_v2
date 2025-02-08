using BuildBlocks.Domain.Atributes;

namespace BuildBlocks.Domain.Exceptions
{
    [ExceptionStatusCode(400)]
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
