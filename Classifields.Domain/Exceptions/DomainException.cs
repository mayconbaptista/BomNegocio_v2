namespace Classifields.Domain.Exceptions;

internal class DomainException : Exception
{
    public List<string>? Errors { get; init; }

    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, List<string> errors) : base(message)
    {
        Errors = errors;
    }
}
