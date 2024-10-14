using AutoMapper;
using BomNegocio.Application.CQRS.Commands.Address;
using BomNegocio.WebAPI.Requests.Address;
using Classifields.Application.DTO;
using Classifields.Application.Interfaces;
using Classifields.WebAPI.Responses.Address;

namespace Classifields.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class AddressController(IAddressService addressService) : ControllerBase
    {
        private readonly IAddressService _addressService = addressService;

        [HttpPost]
        [ProducesResponseType(typeof(uint), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAsync([FromBody] AddressDto address)
        {

            var newAddress = await _addressService.CreateAsync(address);

            return CreatedAtAction(nameof(CreateAsync), new { newAddress.id }, newAddress);
        }

        [HttpGet("{id:int:min(1)}")]
        [ProducesResponseType(typeof(AddressResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] uint id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);

            return this.Ok(this._mapper.Map<AddressDTO, AddressResponse>(address));
        }
    }
}
