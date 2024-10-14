namespace Classifields.Domain.Entities
{
    public sealed class CategoryEntity : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int? ParentCategoryId { get; private set; }

        /* 1..N */
        public CategoryEntity? ParentCategory { get; private set; }
        public IEnumerable<AnnouncementEntity> Announcements { get; private set; } = new List<AnnouncementEntity>();
        public IEnumerable<CategoryEntity> SubCategories { get; set; } = new List<CategoryEntity>();

        public CategoryEntity(string name, string description, int? parentCategoryId = null)
        {
            Name = name;
            Description = description;
            ParentCategoryId = parentCategoryId;
            Validate();
        }

        public override void Validate()
        {
            When(string.IsNullOrWhiteSpace(Name), "Nome é inválido.");
            When(Name.Length < 3 || Name.Length > 60, "O campo nome deve ter de 3 á 60 caracteres.");
            When(string.IsNullOrWhiteSpace(Description), "Descrição é inválida.");
            When(Description.Length < 3 || Description.Length > 500, "O campo descrição deve ter de 3 á 500 caracteres.");
            When(ParentCategoryId != null && ParentCategoryId == 0, "Categoria pai é inválida.");
            When(ParentCategoryId != null && ParentCategoryId == Id, "Categoria pai não pode ser a mesma que a categoria atual.");
            Execute();
        }
    }
}
