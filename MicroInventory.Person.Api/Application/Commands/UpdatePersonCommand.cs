using MediatR;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Person.Api.Application.Commands
{
    public class UpdatePersonCommand:IRequest<Result>
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
    }
}
