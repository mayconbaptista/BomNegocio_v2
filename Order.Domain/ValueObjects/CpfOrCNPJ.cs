using BuildBlocks.Domain.Abstractions;
using System.Text.RegularExpressions;

namespace Order.Domain.ValueObjects
{
    public class CpfOrCNPJ : ValueObject
    {
        public string Value { get; private init; }

        public string Type { get; private init; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }

        public static bool TryParseCpf(string numero, out string cpf) 
        {
            cpf = string.Empty;
            string number = numero.Replace(@"[^0-9]", "");

            if (number.Length != 11) return false;

            int soma = 0;

            foreach (char element in number.Substring(0, 8).AsEnumerable())
            {
                soma += (int)element;
            }

            if (soma % 10 != (int)numero[9]) return false;

            soma += (int)numero[9];

            if(soma % 11 != (int)numero[10]) return false;

            cpf = numero;
            return true;
        }

        public static bool TryParseCNPJ(string numero, out string cnpj)
        {
            cnpj = string.Empty;
            if(numero.Length != 14) return false;

            return true;
        }
    }
}
