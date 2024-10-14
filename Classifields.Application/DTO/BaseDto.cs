using System.Text.Json.Serialization;

namespace Classifields.Application.DTO
{
    public abstract class BaseDto
    {
        [JsonPropertyName("id")] public int Id { get; set; }
    }
}
