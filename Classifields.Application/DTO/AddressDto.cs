namespace Classifields.Application.DTO;

public class AddressDto : BaseDto
{
    [Required(ErrorMessage = "O comapo Rua é obrigatório")]
    [Range(5, 100, ErrorMessage = "O campo Rua deve ter entre 5 e 100 caracteres")]
    [JsonPropertyName("rua")] public string Road { get; set; }
    [MaxLength(100, ErrorMessage = "O campo Complemento deve ter no máximo 100 caracteres")]
    [JsonPropertyName("complemento")] public string Complement { get; set; }
    [Required(ErrorMessage = "O campo Bairro é obrigatório")]
    [Range(5, 100, ErrorMessage = "O campo Bairro deve ter entre 5 e 100 caracteres")]
    [JsonPropertyName("bairro")] public string Neighborhood { get; set; }
    [Required(ErrorMessage = "O campo Cidade é obrigatório")]
    [Range(5, 100, ErrorMessage = "O campo Cidade deve ter entre 5 e 100 caracteres")]
    [JsonPropertyName("cidade")] public string City { get; set; }
    [Required(ErrorMessage = "O campo Estado é obrigatório")]
    [Range(2, 2, ErrorMessage = "O campo Estado deve ter 2 caracteres")]
    [JsonPropertyName("estado")] public string State { get; set; }
    [Required(ErrorMessage = "O campo CEP é obrigatório")]
    [Range(8, 8, ErrorMessage = "O campo CEP deve ter 8 caracteres")]
    [JsonPropertyName("cep")] public string ZipCode { get; set; }
    [Required(ErrorMessage = "O campo Número é obrigatório")]
    [Range(1, 10, ErrorMessage = "O campo Número deve ter entre 1 e 10 caracteres")]
    [JsonPropertyName("numero")] public string Number { get; set; }
}
