using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok("AuthController");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TokenModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenModel>> Login([FromBody] LoginModel model)
        {
            return Ok(await _authService.LoginAsync(model));
        }

        [HttpPost("sing-up")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> SingUp([FromBody] UserCreateDTo user)
        {
            var id = await _authService.RegisterAsync(user);

            return CreatedAtAction("sing-up", new {Id = id});
        }


        [HttpGet("check")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Chech()
        {
            return Ok(new {message = "Checked"});
        }
    }
}
