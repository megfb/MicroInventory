using MediatR;
using MicroInventory.Assignment.Api.Application.Dtos;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.Queries
{
    public class GetAssignmentsQuery:IRequest<IDataResult<IEnumerable<AssignmentDto>>>
    {
    }
}
