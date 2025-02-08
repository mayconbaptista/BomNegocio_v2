using BuildInBlocks.CQRS.Atributes;

namespace BuildInBlocks.CQRS.Excetions
{
    [ExceptionStatusCode(400)]
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string message) : base(message)
        {
        }
    }
}
