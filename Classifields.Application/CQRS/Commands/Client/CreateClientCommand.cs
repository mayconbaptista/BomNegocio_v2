using Classifields.Domain.Entities;

namespace Classifields.Application.CQRS.Commands.Client
{
    public sealed class CreateClientCommand : IRequest<ClientEntity>
    {
        public DateOnly RegistrationDate { get; init; }
        public DateOnly? DeactivationDate { get; private set; }

        public CreateClientCommand(uint userId)
        {
            RegistrationDate = new DateOnly();
            DeactivationDate = null;
        }
    }
}
