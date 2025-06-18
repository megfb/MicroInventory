using MediatR;
using MicroInventory.Assignment.Api.Application.Commands;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;

namespace MicroInventory.Assignment.Api.Application.CommandHandlers
{
    public class DeleteAssignmentCommandHandler(IAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork, ILogger<DeleteAssignmentCommandHandler> logger) : IRequestHandler<DeleteAssignmentCommand, Result>
    {
        private readonly IAssignmentRepository _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteAssignmentCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Id);
            if (assignment == null)
                throw new KeyNotFoundException("Assignment is not found");
            await _assignmentRepository.DeleteAsync(assignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Assignment deleted successfully with ID: {AssignmentId}", assignment.Id);
            return new Result(true, "Assignment successfully deleted");
        }
    }
}
