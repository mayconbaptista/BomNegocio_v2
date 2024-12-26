
using BuildBlocks.Domain.Abstractions;

namespace Order.Domain.Entities
{
    public class ProductEntity : BaseEntity<Guid>
    {
        public string Name { get; private set; }

        public static ProductEntity Create( Guid id,string name, decimal unitPrice)
        {
            return new ProductEntity
            {
                Id = id,
                Name = name
            };
        }
    }
}
