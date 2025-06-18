using MediatR;
using MicroInventory.Category.Api.Application.Commands;
using MicroInventory.Category.Api.Application.Dtos;
using MicroInventory.Category.Api.Application.Queries;
using Microsoft.AspNetCore.Mvc;

namespace MicroInventory.Category.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoriesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(UpdateCategoriesCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryDto>> DeleteCategory(string id)
        {
            return Ok(await _mediator.Send(new DeleteCategoriesCommand { Id = id }));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            return Ok(await _mediator.Send(new GetCategoriesQuery()));
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategory(string id)
        {
            return Ok(await _mediator.Send(new GetCategoryByIdQuery { Id = id }));
        }
    }
}
