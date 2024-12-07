using Microsoft.AspNetCore.Authorization;

namespace Catalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public sealed class ProductController(IProductService productService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IProductService _productService = productService;

        [HttpPost]
        [ProducesResponseType(typeof(ProductListDto) ,StatusCodes.Status201Created)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] ProductListDto productDto)
        {
            var product = _mapper.Map<ProductModel>(productDto);

            var response = _mapper.Map<ProductModel>(await _productService.Save(product));

            return CreatedAtAction(nameof(Create), new { id = response.Id }, response);

        }

        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductListDto>> GetById([FromRoute] uint id)
        {
            var response = await _productService.GetById(id);

            return Ok(_mapper.Map<ProductListDto>(response));
        }

        [HttpGet("life")]
        public IActionResult Get()
        {
            return Ok("Ola anguk");
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(IList<ProductListDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IList<ProductListDto>>> GetAll()
        {
            var response = await _productService.GetAll();

            return Ok(_mapper.Map<IList<ProductListDto>>(response));
        }
    }
}
