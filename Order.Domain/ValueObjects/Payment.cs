
using BuildBlocks.Domain.Abstractions;
using Order.Domain.Enums;
using System.Text.RegularExpressions;

namespace Order.Domain.ValueObjects
{
    public class Payment : ValueObject
    {
        public Currency Currency { get; private set; }
        public PaymentMethod Type { get; private set; }
        public string? CardNumber { get; private set; } = null;
        public string? CardHolderName { get; private set; } = null;
        public DateOnly? CardExpirationDate { get; private set; } = null;
        public string? CardCvv { get; private set; } = null;
        public string? QrCodePayload { get; private set; } = null;


        private Payment(PaymentMethod type, string cardNumber, string cardHolderName, DateOnly? cardExpirationDate, string? cardCvv, string qrCode) 
        {
            this.Type = type;
            this.CardNumber = cardNumber;
            this.CardHolderName = cardHolderName;
            this.CardExpirationDate = cardExpirationDate;
            this.CardCvv = cardCvv;
            this.QrCodePayload = qrCode;
        }

        public static Payment Create(int type, string cardNumber, string cardHolderName, DateOnly? cardExpirationDate, string? cardCvv, string qrCode)
        {
            if (type == (int)PaymentMethod.CREDIT_CARD)
            {
                return CreditCard(cardNumber, cardHolderName, cardExpirationDate.Value, cardCvv);
            }
            else if (type == (int)PaymentMethod.PIX)
            {
                return PIX(qrCode);
            }
            else
            {
                throw new ArgumentException("Tipo de pagamento inválido.");
            }
        }

        public static Payment PIX(string qrCode)
        {
            return new Payment(PaymentMethod.PIX, null, null, null, null, qrCode);
        }

        public static Payment CreditCard(string cardNumber, string cardHolderName, DateOnly cardExpirationDate, string cardCvv)
        {
            string cardNumberRegex = cardNumber.Replace(" ", "");

            var match = Regex.Match(cardNumberRegex, @"(\d{4} \d{4} \d{4} \d{4})");

            if (!match.Success)
                throw new ArgumentException("Número de cartão de crédito inválido. Formato aceito: 16 dígitos - ex: 0000 0000 0000 0000.");

            string cvvRegex = cardCvv.Replace(" ", "");

            match = Regex.Match(cvvRegex, @"(\d{3,4})");

            if (!match.Success)
                throw new ArgumentException("CVV inválido. Formato aceito: 3 ou 4 dígitos - ex: 123 ou 1234.");

            if(cardHolderName.Length > 60)
                throw new ArgumentException("Nome do titular do cartão inválido. O nome deve conter no máximo 60 caracteres.");

            return new Payment(PaymentMethod.CREDIT_CARD, cardNumber, cardHolderName, cardExpirationDate, cardCvv, null);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Currency;
            yield return Type;
            yield return CardNumber;
            yield return CardHolderName;
            yield return CardExpirationDate;
            yield return CardCvv;
        }
    }
}
