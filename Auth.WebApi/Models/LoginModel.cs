using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Auth.WebApi.Models
{
    public sealed class LoginModel
    {
        [Required(ErrorMessage = "É obrigatório")]
        [EmailAddress(ErrorMessage = "Invalido")]
        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "É obrigatório")]
        [DataType(DataType.Password)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
