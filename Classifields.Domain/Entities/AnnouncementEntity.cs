using Classifields.Domain.Enums;

namespace Classifields.Domain.Entities
{
    public sealed class AnnouncementEntity : BaseEntity
    {
        #region "propriedades"
        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public DateTime CreationAt { get; init; }
        public DateTime? DeactivationAt { get; private set; }
        public StatusAnnuncement Status { get; private set; }

        #region "Relations"
        /* EF 1..N */
        public int AdvertiserId { get; private set; }
        public AdvertiserEntity? Advertiser { get; private set; }

        public int CategoryId { get; private set; }
        public CategoryEntity? Category { get; private set; }
        public AddressEntity Address { get; private set; }
        /* EF 1..1 */

        /* EF N..1 */
        public ICollection<EvaluetionEntity>? Evaluetions { get; private set; }
        public ICollection<ImageEntity>? Images { get; private set; }
        public ICollection<WisheEntity>? Wishes { get; private set; }

        #endregion
        #endregion

        #region "Constructors"
        public AnnouncementEntity(
            string title,
            string description,
            decimal price,
            int advertiserId,
            int categoryId,
            object value)
        {
            Title = title;
            Description = description;
            Price = price;
            CreationAt = DateTime.Now;
            DeactivationAt = null;
            Status = StatusAnnuncement.ACTIVE;
            AdvertiserId = advertiserId;
            CategoryId = categoryId;
            Validate();
        }

        public void Deactivate()
        {
            DeactivationAt = DateTime.Now;
            Status = StatusAnnuncement.INACTIVE;
            When(DeactivationAt == default, "Data de desativação é inválida.");
            Validate();
        }

        public void UpdateStatus(StatusAnnuncement status)
        {
            Status = status;
            Validate();
        }
        #endregion

        public override void Validate()
        {
            When(string.IsNullOrWhiteSpace(Title), "Título é inválido.");
            When(Title.Length < 3 || Title.Length > 60, "O campo título deve ter de 3 á 60 caracteres.");
            When(string.IsNullOrWhiteSpace(Description), "Descrição é inválida.");
            When(Description.Length < 3 || Description.Length > 500, "O campo descrição deve ter de 3 á 500 caracteres.");
            When(Price <= 0, "Preço é inválido.");
            When(CreationAt == default, "Data de criação é inválida.");
            When(DeactivationAt != null && DeactivationAt < CreationAt, "Data de desativação não pode ser menor que a data de ativação");
            When(AdvertiserId == 0, "Anunciante é inválido.");
            When(CategoryId == 0, "Categoria é inválida.");
            Execute();
        }
    }
}
