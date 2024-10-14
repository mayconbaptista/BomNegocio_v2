using BomNegocio.Application.Services;
using Classifields.Application.Interfaces;
using MediatR;
using Moq;

namespace BomNegocio.Application.Tests
{
    public class AdressServiceTests
    {
        private readonly IAddressService _addressService;
        private readonly Mock<IMediator> _mediatorMock = new Mock<IMediator>();

        public AdressServiceTests()
        {
            _addressService = new AddressService(new MediatR());
        }

        [Fact(DisplayName = "")]
        public void Test1()
        {

        }

    }
}