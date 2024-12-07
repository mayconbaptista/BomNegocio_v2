
namespace Catalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
    }
}
