
using Catalog.Application.Interfaces;
using Npgsql;

namespace Catalog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ProductController(IProductService productService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IProductService _productService = productService;

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto) ,StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] ProductDto productDto)
        {
            try
            {

                var product = _mapper.Map<ProductModel>(productDto);

                var response = _mapper.Map<ProductModel>(await _productService.Save(product));

                return CreatedAtAction(nameof(Create), new { id = response.Id }, response);

            }
            catch(NpgsqlException ex)
            {
                return BadRequest();
            }
            catch(Exception ex)
            {
                throw;
            }

        }

        [HttpGet("{id:min(1)}")]
        [ProducesResponseType(typeof(ProductModel), StatusCodes.Status200OK)]
        public async Task<ActionResult<ProductModel>> GetById([FromRoute] uint id)
        {
            var response = await _productService.GetById(id);

            return Ok(response);
        }
    }
}
