using BomNegocio.Domain.Entities;
using BomNegocio.Domain.Exceptions;
using FluentAssertions;

namespace BomNegocio.Domain.Tests
{
    public class ClientTests
    {
        [Fact(DisplayName = "Criando cliente com id do usuário nulo.")]
        public void create_client_with_id_invalid_should_throw_exception()
        {
            // Arrange
            Action action = () => new ClientEntity(0);
            action.Should()
                .Throw<DomainException>()
                .Which.Errors?.Contains("O Id do usuário é inválido.");
        }

        [Fact(DisplayName = "Data de desativação invalida")]
        public void create_client_with_deactivation_date_invalid_should_throw_exception()
        {
            // Arrange
            Action action = () => new ClientEntity(1).Deactivate(new DateOnly());
            action.Should()
                .Throw<DomainException>()
                .Which.Errors?.Contains("Data de desativação é inválida.");
        }
    }
}
