using Application.Common.Interfaces;
using Domain.Entities;

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
    }
}