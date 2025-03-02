using BuildBlocks.Domain.Atributes;

namespace BuildBlocks.Domain.Exceptions
{
    [ExceptionStatusCode(404)]
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
