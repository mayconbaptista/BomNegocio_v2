using System;
using System.ComponentModel.DataAnnotations;


namespace BuildBlocks.Domain.Exceptions
{
    public class DomainValidationException : ValidationException
    {

        public DomainValidationException(string message) : base(message)
        {

        }
    }
}
