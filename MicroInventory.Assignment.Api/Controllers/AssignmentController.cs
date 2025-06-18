using MediatR;
using MicroInventory.Assignment.Api.Application.Commands;
using MicroInventory.Assignment.Api.Application.Dtos;
using MicroInventory.Assignment.Api.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroInventory.Assignment.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost]
        public async Task<ActionResult<AssignmentDto>> CreateAssignment(CreateAssignmentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AssignmentDto>> UpdateAssignment(UpdateAssignmentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPut("Drop/{id}")]
        public async Task<ActionResult<AssignmentDto>> DropAssignment(DropAssignmentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AssignmentDto>> DeleteAssignment(string id)
        {
            return Ok(await _mediator.Send(new DeleteAssignmentCommand { Id = id }));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssignmentDto>>> GetAllAssignments()
        {
            return Ok(await _mediator.Send(new GetAssignmentsQuery()));
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<AssignmentDto>> GetAssignment(string id)
        {
            return Ok(await _mediator.Send(new GetAssignmentByIdQuery { Id = id }));
        }
    }
}
