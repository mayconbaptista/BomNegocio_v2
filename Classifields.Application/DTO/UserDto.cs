namespace Classifields.Application.DTO
{
    public class UserDto : BaseDto
    {
        [Required(ErrorMessage = "O campo nome é obrigatório.")]
        [Range(5, 100, ErrorMessage = "O campo nome deve ter entre 5 e 100 caracteres.")]
        [JsonPropertyName("nome")]
        public string Name { get; set; }
        [Required(ErrorMessage = "O campo email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo email deve ser um email válido.")]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo Cpf é obrigatório.")]
        [Range(11, 11, ErrorMessage = "O campo Cpf deve ter 11 caracteres.")]
        [JsonPropertyName("cpf")]
        public string Cpf { get; set; }
        [JsonPropertyName("telefone")]
        public string PhoneNumber { get; set; }
        public ICollection<AddressDto>? Addresses { get; set; }
        public ICollection<ImageDto>? Images { get; set; }
    }
}
