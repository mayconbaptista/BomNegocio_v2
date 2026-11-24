
using BuildBlocks.Domain.Abstractions;
using Order.Domain.Enums;
using Order.Domain.Exceptions;
using Order.Domain.Extensions;

namespace Order.Domain.Entities
{
    public class PaymentEntity : BaseEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public string PxId { get; private set; }
        public Guid Key { get; private set; }
        public PaymentMethod PaymentMethod { get; private set; }
        public DateTime ExpireIn { get; private set; }
        public string PixCopyAndPaste { get; private set; }
        public string Location { get; private set; }

        public static PaymentEntity Pix(string pxId, Guid key, DateTime createAt, TimeSpan expireIn, string pixCopy, string location)
        {
            List<string> errors = new List<string>();

            errors.AddIf(string.IsNullOrEmpty(pxId), "O campo pxId é obrigatório.")
                .AddIf(key == Guid.Empty, "O campo chave é obrigatório.")
                .AddIf(string.IsNullOrEmpty(pixCopy), "O campo pix copia e cola é obrigatório.")
                .AddIf(string.IsNullOrEmpty(location), "O campo localização é obrigatório.")
                .AddIf(expireIn.TotalSeconds <= 0, "O tempo de espiração deve ser maior que zero.");

            DomainException.ThrowIfAnyErro(errors, "Dados de pagamentos gerados são invaidos");

            return new PaymentEntity()
            {
                PxId= pxId,
                PaymentMethod= PaymentMethod.PIX,
                Key= key,
                ExpireIn= createAt.AddSeconds(expireIn.TotalSeconds),
                PixCopyAndPaste= pixCopy,
                Location=location
            };
        }
    }
}
