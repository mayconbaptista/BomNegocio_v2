
using BuildBlocks.Domain.Abstractions;
using System.Text.RegularExpressions;
using FluentValidation;

namespace Order.Domain.ValueObjects
{
    public class SkuCode : ValueObject
    {
        public string Value { get; private init; }


        public SkuCode(string value)
        {
            if(Check(value)) throw new ValidationException("O código do produto deve conter apenas letras e números");

            Value = value;
        }

        /// <summary>
        /// Conversão implicida de string para PhoneNumber
        /// </summary>
        /// <param name="skuCode"></param>
        public static implicit operator SkuCode(string skucode) => new SkuCode(skucode);

        /// <summary>
        /// Conversão implicida de PhoneNumber para string
        /// PhoneNumber phoneNumber = "(11) 99999-9999"
        /// string strPhoneNumber = phoneNumber -> A string strPhoneNumber contem o valor "(11) 99999-9999"
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static implicit operator string(SkuCode skuCode) => skuCode.ToString();

        public static bool Check(string phoneNumber)
        {
            string number = phoneNumber.Replace(" ", "");

            var match = Regex.Match(number, @"^[a-zA-Z0-9]*$");

            return match.Success;
        }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
