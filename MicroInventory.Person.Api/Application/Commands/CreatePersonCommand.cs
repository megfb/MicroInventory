using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Person.Api.Application.Commands
{
    public class CreatePersonCommand:IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Department { get; set; }  
    }
}
