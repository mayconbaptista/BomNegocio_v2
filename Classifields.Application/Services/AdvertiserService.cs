using AutoMapper;
using Classifields.Application.Interfaces;
using Classifields.Domain.Entities;

namespace Classifields.Application.Services;

public sealed class AdvertiserService(IMediator mediator) : IAdvertiserService
{
    private readonly IMediator _mediator = mediator;

    public async Task<IEnumerable<AdvertiserEntity>> GetAdvertisers()
    {
        throw new NotImplementedException();
    }
}
