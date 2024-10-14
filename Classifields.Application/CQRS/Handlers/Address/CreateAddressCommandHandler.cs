using Classifields.Application.CQRS.Commands.Address;
using Classifields.Application.CQRS.Handlers;
using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;

namespace Classifields.Application.CQRS.Handlers.Address;

public class CreateAddressCommandHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateAddressCommand, AddressEntity>
{
    private readonly IAddressRepository _addressRepository = unitOfWork.AddressRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AddressEntity> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var newAddress = new AddressEntity(
            request.Street,
            request.Complement,
            request.Neighborhood,
            request.City,
            request.State,
            request.ZipCode,
            request.Country,
            request.Number,
            request.ClientId,
            null);

        var entt = await _addressRepository.CreateAsync(newAddress);
        await _unitOfWork.CommitAsync();
        return entt;
    }
}
