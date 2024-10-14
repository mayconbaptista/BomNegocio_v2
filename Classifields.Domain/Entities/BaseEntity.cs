using System.ComponentModel.DataAnnotations.Schema;

namespace Classifields.Domain.Entities
{
    public abstract class BaseEntity
    {
        public uint Id { get; protected set; }

        [NotMapped]
        public List<string> Erros { get; private set; } = new List<string>();

        protected void When(bool hasError, string error)
        {
            if (hasError) Erros.Add(error);
        }

        protected void Execute()
        {
            if (Erros.Any())
            {
                throw new DomainException("Erro de validação.", Erros);
            }
        }

        public abstract void Validate();
    }
}
