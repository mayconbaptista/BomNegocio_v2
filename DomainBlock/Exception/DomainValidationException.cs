using System;
using System.ComponentModel.DataAnnotations;


namespace DomainBlock.Exception
{
    public class DomainValidationException : ValidationException
    {

        public DomainValidationException(string message) : base(message)
        {

        }
    }
}
