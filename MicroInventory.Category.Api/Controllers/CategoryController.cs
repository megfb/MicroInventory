using MediatR;
using MicroInventory.Category.Api.Application.Commands;
using MicroInventory.Category.Api.Application.Dtos;
using MicroInventory.Category.Api.Application.Queries;
using Microsoft.AspNetCore.Http;
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
            await _mediator.Send(command);
            return Ok("Everything is ok");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(UpdateCategoriesCommand command)
        {
            await _mediator.Send(command);
            return Ok("Everything is ok");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CategoryDto>> DeleteCategory(Guid id)
        {
            await _mediator.Send(new DeleteCategoriesCommand { Id = id });
            return Ok("Everything is ok");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(categories);
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategory(Guid id)
        {
            var categories = await _mediator.Send(new GetCategoryByIdQuery { Id = id});
            return Ok(categories);
        }
    }
}
