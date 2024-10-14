using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Classifields.Application.DTO
{
    public class AnnouncementDto : BaseDto
    {
        [Required(ErrorMessage = "O Atributo titulo é obrigatório.")]
        [JsonPropertyName("titulo")]
        public string Title { get; set; }
        [MaxLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        [JsonPropertyName("descricao")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "O atributo preço é obrigatório")]
        [JsonPropertyName("preco")]
        public decimal Price { get; set; }
        [JsonPropertyName("dataCriacao")]
        public DateTime CreationDate { get; set; }
        [JsonPropertyName("dataDesativacao")]
        public DateTime DeactivationDate { get; set; }
    }
}
