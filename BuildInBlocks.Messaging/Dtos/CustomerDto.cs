using System.ComponentModel.DataAnnotations;

namespace BuildInBlocks.Messaging.Dtos;

public record CustomerDto([Required] string Documento, [Required] string Name,[Required] string Email);