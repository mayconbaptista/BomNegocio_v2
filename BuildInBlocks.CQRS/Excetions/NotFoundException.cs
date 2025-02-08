

using BuildInBlocks.CQRS.Atributes;

namespace BuildInBlocks.CQRS.Excetions
{
    [ExceptionStatusCode(404)]
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
