using MediatR;
using MicroInventory.Assignment.Api.Application.Dtos;
using MicroInventory.Assignment.Api.Application.Queries;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.QueryHandlers
{
    public class GetAssignmentByIdQueryHandler(IAssignmentRepository assignmentRepository, ILogger<GetAssignmentByIdQueryHandler> logger) : IRequestHandler<GetAssignmentByIdQuery, IDataResult<AssignmentDto>>
    {
        private readonly IAssignmentRepository _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        private readonly ILogger<GetAssignmentByIdQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<IDataResult<AssignmentDto>> Handle(GetAssignmentByIdQuery request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Id);
            if (assignment == null)
                throw new KeyNotFoundException("Assignment is not found");

            var assignmentDto = new AssignmentDto
            {
                Id = assignment.Id,
                PersonId = assignment.PersonId,
                ProductId = assignment.ProductId,
                Notes = assignment.Notes,
                AssignedAt = assignment.AssignedAt,
                ReturnedAt = assignment.ReturnedAt,
                CreatedAt = assignment.CreatedAt,
                UpdatedAt = assignment.UpdatedAt
            };
            _logger.LogInformation("Retrieved assignment with ID {AssignmentId}", request.Id);
            return new DataResult<AssignmentDto>(true, "Assignment is ", assignmentDto);
        }
    }
}
