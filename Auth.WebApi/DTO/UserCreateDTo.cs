using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Auth.WebApi.DTO
{
    public class UserCreateDTo
    {
        [Required(ErrorMessage = "É obrigatório.")]
        public string UserName { get; set; } 

        [Required(ErrorMessage = "É obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "É obrigatório.")]
        [Phone(ErrorMessage = "Telefone inválido")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "É obrigatório.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "É obrigatório.")]
        [Compare("Password", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmPassword { get; set; }

        //public bool ShopKeeper { get; set; }

    }
}
