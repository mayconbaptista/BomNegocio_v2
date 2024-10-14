using AutoMapper;

namespace Classifields.WebAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAdsress, Response.AddressResponse>();
        }
    }
}
