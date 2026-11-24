using BuildBlocks.Domain.Exceptions;
using Order.Domain.Enums;

namespace Order.Domain.Exceptions
{
    public class DomainException : BaseException
    {
        public override int ErrorCode => (int)ErrorCodeType.EntityInvariantViolation;

        public DomainException(string message) : base(message) { }

        public DomainException(string message, List<string> errors) : base(message, errors) { }

        static public void ThrowIfAnyErro(List<string> erros, string message)
        {
            if (erros.Count > 0)
            {
                throw new DomainException(message, erros);
            }
        }
    }
}
