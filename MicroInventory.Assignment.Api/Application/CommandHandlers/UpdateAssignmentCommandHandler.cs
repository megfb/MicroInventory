using MediatR;
using MicroInventory.Assignment.Api.Application.Commands;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.CommandHandlers
{
    public class UpdateAssignmentCommandHandler(IAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork, ILogger<UpdateAssignmentCommandHandler> logger) : IRequestHandler<UpdateAssignmentCommand, Result>
    {
        private readonly IAssignmentRepository _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<UpdateAssignmentCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(UpdateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Id);

            if (assignment == null)
                throw new KeyNotFoundException("Assignment is not found");
            assignment.ProductId = request.ProductId;
            assignment.PersonId = request.PersonId;
            assignment.Notes = request.Notes;
            assignment.UpdatedAt = DateTime.UtcNow;
            await _assignmentRepository.UpdateAsync(assignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Assignment updated successfully with ID: {AssignmentId}", assignment.Id);
            return new Result(true, "Assignment successfully updated");
        }
    }
}
