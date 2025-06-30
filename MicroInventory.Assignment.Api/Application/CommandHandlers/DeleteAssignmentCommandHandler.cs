using MediatR;
using MicroInventory.Assignment.Api.Application.Commands;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Assignment.Api.Application.CommandHandlers
{
    public class DeleteAssignmentCommandHandler(IAssignmentRepository assignmentRepository, IUnitOfWork unitOfWork, ILogger<DeleteAssignmentCommandHandler> logger, IEventBus eventBus) : IRequestHandler<DeleteAssignmentCommand, Result>
    {
        private readonly IAssignmentRepository _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<DeleteAssignmentCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        public async Task<Result> Handle(DeleteAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(request.Id);
            if (assignment == null)
                throw new KeyNotFoundException("Assignment is not found");
            await _assignmentRepository.DeleteAsync(assignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Assignment deleted successfully with ID: {AssignmentId}", assignment.Id);
            await _eventBus.PublishAsync(new AssignmentDeletedIntegrationEvent { ProductId = assignment.ProductId }, "assignment-events-topic");
            return new Result(true, "Assignment successfully deleted");
        }
    }
}
