
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Order.Domain.Interfaces;

namespace Order.Application.CQRS.Order.Queries;

public class ListAllOrdersQuery : IRequest<List<OrderDto>>
{
    public Guid CustomerId { get; init; } = default!;
}

public class ListAllOrdersQueryHandler(IUnitOfWork unitOfWork,ILogger<ListAllOrdersQueryHandler> logger) 
    : IRequestHandler<ListAllOrdersQuery, List<OrderDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ListAllOrdersQueryHandler> _logger = logger;

    public async Task<List<OrderDto>> Handle(ListAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await this._unitOfWork.OrderRepository.GetAllAsync(x => x.CustomerId == request.CustomerId, asTraking:false);

        return orders.Select(x => x.Adapt<OrderDto>()).ToList();
    }
}