using MediatR;
using MicroInventory.Assignment.Api.Application.Dtos;
using MicroInventory.Assignment.Api.Application.Queries;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.QueryHandlers
{
    public class GetAssignmentsQueryHandler(IAssignmentRepository assignmentRepository, ILogger<GetAssignmentsQueryHandler> logger) : IRequestHandler<GetAssignmentsQuery, IDataResult<IEnumerable<AssignmentDto>>>
    {
        private readonly IAssignmentRepository _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        private readonly ILogger<GetAssignmentsQueryHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<IDataResult<IEnumerable<AssignmentDto>>> Handle(GetAssignmentsQuery request, CancellationToken cancellationToken)
        {
            var assignments = await _assignmentRepository.GetAllAsync();
            var assignmentDtos = assignments.Select(a => new AssignmentDto
            {
                Id = a.Id,
                PersonId = a.PersonId,
                ProductId = a.ProductId,
                Notes = a.Notes,
                AssignedAt = a.AssignedAt,
                ReturnedAt = a.ReturnedAt,
                CreatedAt = a.CreatedAt,
                UpdatedAt = a.UpdatedAt
            }).ToList();
            _logger.LogInformation("Retrieved {Count} assignments", assignmentDtos.Count);
            return new DataResult<IEnumerable<AssignmentDto>>(true, "Assignments retrieved successfully", assignmentDtos);
        }
    }
}
