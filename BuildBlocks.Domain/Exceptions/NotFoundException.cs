using BuildBlocks.Domain.Atributes;
using System.Reflection;

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
