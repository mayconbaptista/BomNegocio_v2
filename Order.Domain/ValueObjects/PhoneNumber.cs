using BuildBlocks.Domain.Abstractions;
using System.Text.RegularExpressions;

namespace Order.Domain.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string DDD { get; private set; }
        public string Number { get; private set; }


        public PhoneNumber(string phoneNumber)
        {
            var (ddd, number) = Extract(phoneNumber);

            this.DDD = ddd;
            this.Number = number;
        }

        /// <summary>
        /// Conversão implicida de string para PhoneNumber ex:
        /// PhoneNumber phoneNumber = "(11) 99999-9999";
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static implicit operator PhoneNumber(string phoneNumber) => new PhoneNumber(phoneNumber);

        /// <summary>
        /// Conversão implicida de PhoneNumber para string
        /// PhoneNumber phoneNumber = "(11) 99999-9999"
        /// string strPhoneNumber = phoneNumber -> A string strPhoneNumber contem o valor "(11) 99999-9999"
        /// </summary>
        /// <param name="phoneNumber"></param>
        public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.ToString();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return DDD;
            yield return Number;
        }

        /// <summary>
        /// Extrai o DDD e o número do telefone de uma string
        /// </summary>
        /// <param name="phoneNumber">string com um numero de telefone celular válido</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static (string, string) Extract(string phoneNumber)
        {
            string number = phoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");

            var match = Regex.Match(number, @"^(\d{2})(\d{8,9})$");

            if (!match.Success) 
                throw new ArgumentException("Telefone celular inválido  formatos aceitos -> +" +
                    "(99) ? 99999-9999, " +
                    "(99) ?9999-9999, " +
                    "(99) ?99999999" +
                    "(99)?99999999");

            return (match.Groups[1].Value, match.Groups[2].Value);
        }

        public override string ToString() => $"{DDD}{Number}";

        public string ToString(string format, IFormatProvider formatProvider) => ToString();
    }
}
