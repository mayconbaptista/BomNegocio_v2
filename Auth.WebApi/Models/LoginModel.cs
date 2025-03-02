using System.ComponentModel.DataAnnotations;

namespace Auth.WebApi.Models
{
    public sealed class LoginModel
    {
        [Required(ErrorMessage = "É obrigatório")]
        [EmailAddress(ErrorMessage = "Invalido")]
        public string UserEmail { get; set; }
        [Required(ErrorMessage = "É obrigatório")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
