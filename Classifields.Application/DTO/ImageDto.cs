namespace Classifields.Application.DTO
{
    public class ImageDto : BaseDto
    {
        [JsonPropertyName("path")]
        public string Path { get; set; }

        /* 1..n */
        [JsonPropertyName("anuncioId")]
        public int AnnouncementId { get; set; }
        [JsonPropertyName("anuncio")]
        public AnnouncementDto? Announcement { get; set; }
        [JsonPropertyName("userId")]
        public int UserId { get; set; }
        [JsonPropertyName("usuario")]
        public UserDto? User { get; set; }
    }
}