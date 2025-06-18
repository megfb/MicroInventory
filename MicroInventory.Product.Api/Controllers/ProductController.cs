using MediatR;
using MicroInventory.Product.Api.Application.Commands;
using MicroInventory.Product.Api.Application.Dtos;
using MicroInventory.Product.Api.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroInventory.Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(UpdateProductsCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDto>> DeleteProduct(string id)
        {
            return Ok(await _mediator.Send(new DeleteProductsCommand { Id = id }));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            return Ok(await _mediator.Send(new GetProductsQuery()));
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            return Ok(await _mediator.Send(new GetProductByIdQuery { Id = id }));
        }
    }
}
