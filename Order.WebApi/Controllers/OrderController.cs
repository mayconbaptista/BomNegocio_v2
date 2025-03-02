using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order.Application.CQRS.Order.Commands;

namespace Order.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(ISender sender, ILogger<OrderController> logger) : ControllerBase
    {
        private readonly ISender _sender = sender;
        private readonly ILogger<OrderController> _logger = logger;


        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Guid>> Create(CreateOrderCommand command)
        {
            var Id = await _sender.Send(command);

            return CreatedAtAction(nameof(Create), new { id = Id });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult get()
        {
            return Ok(new { message =  "Message" });
        }

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult list()
        {
            return Ok(new { message = "List" });
        }
    }
}
