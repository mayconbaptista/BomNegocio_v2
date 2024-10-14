using System.ComponentModel.DataAnnotations;

namespace Classifields.Application.DTO
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        [Range(5, 100, ErrorMessage = "Email must be between 5 and 100 characters")]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Range(8, 12, ErrorMessage = "Password must be between 8 and 12 characters")]
        [DataType(DataType.Password)]
        [JsonPropertyName("senha")]
        public string Password { get; set; }
    }
}
