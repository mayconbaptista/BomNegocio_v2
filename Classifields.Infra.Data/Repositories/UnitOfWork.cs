using Classifields.Domain.Interfaces;
using Classifields.Infra.Data.Context;

namespace Classifields.Infra.Data.Repositories;

public sealed class UnitOfWork(BNContext bNContext) : IUnitOfWork
{
    public readonly BNContext _bNContext = bNContext;

    private IAdvertiserRepository? _advertiserRepository;
    private IAnnouncementRepository? _announcementRepository;
    private IEvaluetionRepository? _avaluetionRepository;
    private ICategoryRepository? _categoryRepository;
    private IClientRepository? _clientRepository;
    private IWisheRepository? _wisheRepository;
    private IAddressRepository? _addressRepository;
    private IImageRepository? _imageRepository;
    private IUserRepository? _userRepository;

    public IAdvertiserRepository AdvertiserRepository
    {
        get
        {
            if (_advertiserRepository is null)
            {
                _advertiserRepository = new AdvertiserRepository(_bNContext);
            }
            return _advertiserRepository;
        }
    }

    public IAnnouncementRepository AnnouncementRepository
    {
        get
        {
            if (_announcementRepository is null)
            {
                _announcementRepository = new AnnouncementRepository(_bNContext);
            }
            return _announcementRepository;
        }
    }

    public IEvaluetionRepository EvaluetionRepository
    {
        get
        {
            if (_avaluetionRepository is null)
            {
                _avaluetionRepository = new EvaluetionRepository(_bNContext);
            }
            return _avaluetionRepository;
        }
    }

    public ICategoryRepository CategoryRepository
    {
        get
        {
            if (_categoryRepository is null)
            {
                _categoryRepository = new CategoryRepository(_bNContext);
            }
            return _categoryRepository;
        }
    }

    public IClientRepository ClientRepository
    {
        get
        {
            if (_clientRepository is null)
            {
                _clientRepository = new ClientRepository(_bNContext);
            }
            return _clientRepository;
        }
    }

    public IWisheRepository WisheRepository
    {
        get
        {
            if (_wisheRepository is null)
            {
                _wisheRepository = new WisheRepository(_bNContext);
            }
            return _wisheRepository;
        }
    }

    public IAddressRepository AddressRepository
    {
        get
        {
            if (_addressRepository is null)
            {
                _addressRepository = new AddressRepository(_bNContext);
            }
            return _addressRepository;
        }
    }

    public IImageRepository ImageRepository
    {
        get
        {
            if (_imageRepository is null)
            {
                _imageRepository = new ImageRepository(_bNContext);
            }
            return _imageRepository;
        }
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository is null)
            {
                _userRepository = new UserRepository(_bNContext);
            }
            return _userRepository;
        }
    }

    public async Task CommitAsync()
    {
        await _bNContext.SaveChangesAsync();
    }

    public void Commit()
    {
        _bNContext.SaveChanges();
    }

    public void Rollback()
    {
        _bNContext.Dispose();
    }
}
