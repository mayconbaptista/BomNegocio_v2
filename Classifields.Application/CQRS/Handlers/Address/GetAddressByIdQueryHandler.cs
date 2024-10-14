using Classifields.Application.CQRS.Handlers;
using Classifields.Application.CQRS.Queryes.Address;
using Classifields.Domain.Entities;
using Classifields.Domain.Interfaces;

namespace Classifields.Application.CQRS.Handlers.Address;

internal sealed class GetAddressByIdQueryHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetAddressByIdQuery, AddressEntity>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<AddressEntity> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
    {
        var address = await _unitOfWork.AddressRepository
            .FindAsync(x => x.Id == request.Id) ?? throw new NotFoundException("Address not found");

        return address;
    }
}
