using MediatR;
using MicroInventory.Assignment.Api.Application.Commands;
using MicroInventory.Assignment.Api.Domain.Entities;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Shared.Common.Domain;
using MicroInventory.Shared.Common.Response;
using MicroInventory.Shared.EventBus.Abstractions;
using MicroInventory.Shared.EventBus.Events;

namespace MicroInventory.Assignment.Api.Application.CommandHandlers
{
    public class CreateAssignmentCommandHandler(IAssignmentRepository assignmentRepository,
        IUnitOfWork unitOfWork, ILogger<CreateAssignmentCommandHandler> logger,IEventBus eventBus) : IRequestHandler<CreateAssignmentCommand, Result>
    {
        private readonly IEventBus _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        private readonly IAssignmentRepository _assignmentRepository = assignmentRepository ?? throw new ArgumentNullException(nameof(assignmentRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly ILogger<CreateAssignmentCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        public async Task<Result> Handle(CreateAssignmentCommand request, CancellationToken cancellationToken)
        {
            var assignment = new Assignments()
            {
                Id = Guid.NewGuid().ToString(),
                PersonId = request.PersonId,
                ProductId = request.ProductId,
                Notes = request.Notes,
                AssignedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            await _assignmentRepository.CreateAsync(assignment);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Assignment created successfully with ID: {AssignmentId}", assignment.Id);
            await _eventBus.PublishAsync(new AssignmentAddedIntegrationEvent { ProductId = assignment.ProductId },"assignment-events-topic");
            return new Result(true, "Assignment successfully created");

        }
    }
}
