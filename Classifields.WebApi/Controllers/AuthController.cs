using Microsoft.AspNetCore.Mvc;

namespace Classifields.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticate _authService;
        public AuthController(IAuthenticate authService)
        {
            _authService = authService;
        }
    }
}
