using Classifields.Domain.Entities;

namespace Classifields.Application.DTO
{
    public class AdvertiserDto : UserDto
    {
        [JsonPropertyName("anuncios")]
        public ICollection<AnnouncementEntity>? Announcements { get; set; }
    }
}
