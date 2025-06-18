using MediatR;
using MicroInventory.Assignment.Api.Application.Commands;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.CommandHandlers
{
    public class DropAssignmentCommandHandler(IAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork, ILogger<DropAssignmentCommandHandler> logger) : IRequestHandler<DropAssignmentCommand, Result>
    {
        private readonly IAssignmentRepository _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DropAssignmentCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(DropAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Id);
            if (assignment == null)
                throw new KeyNotFoundException("Assignment is not found");
            assignment.ReturnedAt = DateTime.UtcNow;
            assignment.UpdatedAt = DateTime.UtcNow;

            await _assignmentRepository.UpdateAsync(assignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Assignment dropped successfully with ID: {AssignmentId}", assignment.Id);
            return new Result(true, "Assignment successfully dropped");

        }
    }
}
