using BomNegocio.Domain.Entities;
using BomNegocio.Domain.Exceptions;
using FluentAssertions;

namespace BomNegocio.Domain.Tests
{
    public class UserTests
    {
        [Fact(DisplayName = "Usuário criação com sucesso")]
        public void Create_User_With_Success()
        {
            // Arrange
            Action action = () => new UserEntity("Maycon douglas", "mayconbaptista@ufu.br", "14923266660");
        }

        [Fact(DisplayName = "Usuário criação com nome inválido")]
        public void Create_User_With_Invalid_Name()
        {
            // Arrange
            Action action = () => new UserEntity("MD", "mayconbaptista@ufu.br", "14923266660");
            action.Should()
                .Throw<DomainException>()
                .Which.Errors?.Contains("O nome deve ter entre 3 e 100 caracteres.");
        }

        [Fact(DisplayName = "Usuário criação com email inválido")]
        public void Create_User_With_Invalid_Email()
        {
            // Arrange
            Action action = () => new UserEntity("Maycon douglas", "mayconbaptista", "14923266660");
            action.Should()
                .Throw<DomainException>()
                .Which.Errors?.Contains("Email é inválido.");

            action = () => new UserEntity()
            {
                Name = "Maycon douglas",
                Email = "",
                Cpf = "14923266660"
            }.Validate();
        }
    }
}
