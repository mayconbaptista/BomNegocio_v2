
using BomNegocio.Domain.Entities;
using BomNegocio.Domain.Exceptions;
using FluentAssertions;

namespace BomNegocio.Domain.Tests;

public class AddressTests
{
    [Fact(DisplayName = "Teste de criação de endereço com sucesso")]
    public void create_with_success()
    {
        Action action = () => new AddressEntity("Av. rio branco", "Apartamento 201", "Santa mônica", "Uberlândia", "MG", "38400014", 38, 1, null);

        action.Should().NotThrow();
    }

    [Fact(DisplayName = "Teste de criação de endereço com logradouro inválido")]
    public void create_with_invalid_street()
    {
        Action action = () => new AddressEntity("", "Apartamento 201", "Santa mônica", "Uberlândia", "MG", "38400", 38, 1, null);

        action.Should().Throw<DomainException>().Which.Errors?.Contains("O logradouro é inválido.");
    }

    [Fact(DisplayName = "Teste de criação de endereço com bairro inválido")]
    public void create_with_invalid_neighborhood()
    {
        Action action = () => new AddressEntity("Av. rio branco", "Apartamento 201", "", "Uberlândia", "MG", "38400", 38, 1, null);

        action.Should().Throw<DomainException>().Which.Errors?.Contains("O bairro é inválido.");
    }
}
