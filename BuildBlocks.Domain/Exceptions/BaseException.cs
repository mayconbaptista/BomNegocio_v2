
namespace BuildBlocks.Domain.Exceptions
{
    public abstract class BaseException : Exception
    {
        public abstract int ErrorCode { get; }
        public List<string>? Errors { get; init; } = null;

        protected BaseException(string message, List<string>? erros = null) : base(message)
        {
            this.Errors = erros;
        }
        protected BaseException(string message, List<string>? errors, Exception? innerException) : base(message, innerException)
        {
            Errors = errors;
        }
    }
}
