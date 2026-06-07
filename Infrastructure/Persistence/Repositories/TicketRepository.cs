using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TicketRepository(ApplicationDbContext dbContext) : ITicketRepository
    {
        public async Task<int> AddAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            await dbContext.Tickets.AddAsync(ticket, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return ticket.Id;
        }

        public async Task<IEnumerable<Ticket>> GetAllWithDetailsAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Tickets
                .Include(t => t.Category)
                .Include(t => t.Status)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}