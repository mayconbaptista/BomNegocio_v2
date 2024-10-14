using Classifields.Application.CQRS.Commands.Address;
using Classifields.Application.DTO;

namespace Classifields.Application.Interfaces;

public interface IAddressService
{
    public Task<AddressDto> CreateAsync(AddressDto addressDto);
    public Task<AddressDto> GetByIdAsync(uint id);
}
