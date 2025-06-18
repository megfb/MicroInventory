using MediatR;
using MicroInventory.Person.Api.Application.Commands;
using MicroInventory.Person.Api.Application.Dtos;
using MicroInventory.Person.Api.Application.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroInventory.Person.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        [HttpPost]
        public async Task<ActionResult<PersonDto>> CreatePerson(CreatePersonCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<PersonDto>> UpdatePerson(UpdatePersonCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonDto>> DeletePerson(string id)
        {
            return Ok(await _mediator.Send(new DeletePersonCommand { Id = id }));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetAllPersons()
        {
            return Ok(await _mediator.Send(new GetPersonsQuery()));
        }
        [HttpGet("Get/{id}")]
        public async Task<ActionResult<PersonDto>> GetPerson(string id)
        {
            return Ok(await _mediator.Send(new GetPersonByIdQuery { Id = id }));
        }


    }
}
