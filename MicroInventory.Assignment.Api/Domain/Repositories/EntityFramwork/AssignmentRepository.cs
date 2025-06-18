using MicroInventory.Assignment.Api.Domain.Entities;
using MicroInventory.Assignment.Api.Domain.Repositories.Abstractions;
using MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork.DbContexts;
using MicroInventory.Shared.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Assignment.Api.Domain.Repositories.EntityFramwork
{
    public class AssignmentRepository(AssignmentDbContext context, IUnitOfWork unitOfWork) : IAssignmentRepository
    {
        public async Task<Assignments> CreateAsync(Assignments assignments)
        {
            await context.Assignments.AddAsync(assignments);
            await unitOfWork.SaveChangesAsync();
            return assignments;

        }

        public async Task DeleteAsync(Assignments assignments)
        {
            context.Assignments.Remove(assignments);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Assignments>> GetAllAsync()
        {
            return await context.Assignments.ToListAsync();
        }

        public async Task<Assignments> GetByIdAsync(string id)
        {
            return await context.Assignments
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        public async Task UpdateAsync(Assignments assignments)
        {
            context.Assignments.Update(assignments);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
