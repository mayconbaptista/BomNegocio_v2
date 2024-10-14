namespace Catalog.Domain.Models
{
    public abstract class BaseModel
    {
        public uint Id { get; set; }

        public abstract void Validate();
    }
}
