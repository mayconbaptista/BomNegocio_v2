

namespace BuildBlocks.Domain.ValueObjects
{
    // ref: https://www.youtube.com/watch?v=3EBb8zGJCP4
    public sealed class Email : ValueObject
    {
        public string Value { get; private init; }

        public Email(string value)
        {
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        /// <summary>
        /// Implicit conversion from Email to string
        /// </summary>
        /// <param name="email"></param>
        public static implicit operator string(Email email) => email.Value;


        /// <summary>
        /// Implicit conversion from string to Email
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Email(string value) => new Email(value);
    }
}
