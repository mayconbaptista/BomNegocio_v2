using Classifields.Application.CQRS.Commands.Address;
using Classifields.Application.DTO;
using Classifields.Application.Interfaces;

namespace Classifields.Application.Services;

public sealed class AddressService(IMediator mediator, IMapper mapper) : IAddressService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    public async Task<AddressDto> CreateAsync(AddressDto addressDto)
    {
        var command = _mapper.Map<CreateAddressCommand>(addressDto);

        var entt = await _mediator.Send(command);

        return _mapper.Map<AddressDto>(entt);
    }

    public async Task<AddressDto> GetByIdAsync(uint id)
    {
        var address = await _mediator.Send(new GetAddressByIdQuery(id));

        return _mapper.Map<AddressDto>(address);
    }
}
