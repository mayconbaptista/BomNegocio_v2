using System;
using System.ComponentModel.DataAnnotations;


namespace BuildBlock.Domain.Exception
{
    public class DomainValidationException : ValidationException
    {

        public DomainValidationException(string message) : base(message)
        {

        }
    }
}
