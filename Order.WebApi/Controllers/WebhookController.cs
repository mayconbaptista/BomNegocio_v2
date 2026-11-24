using Microsoft.AspNetCore.Mvc;

namespace Order.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        [HttpPost("notify/{key:guid}")]
        public IActionResult Get([FromRoute] Guid key)
        {
            return Ok($"menssagem recebida chave: {key} atualizada com sucesso.");
        }
    }
}
