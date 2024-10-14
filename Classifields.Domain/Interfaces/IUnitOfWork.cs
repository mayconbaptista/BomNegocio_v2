namespace Classifields.Domain.Interfaces;

public interface IUnitOfWork
{
    public IAdvertiserRepository AdvertiserRepository { get; }
    public IAnnouncementRepository AnnouncementRepository { get; }
    public IEvaluetionRepository EvaluetionRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public IClientRepository ClientRepository { get; }
    public IWisheRepository WisheRepository { get; }
    public IAddressRepository AddressRepository { get; }
    public IImageRepository ImageRepository { get; }
    public IUserRepository UserRepository { get; }

    public Task CommitAsync();
    public void Rollback();
}
