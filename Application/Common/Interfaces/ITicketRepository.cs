using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ITicketRepository
    {
        Task<int> AddAsync(Ticket ticket, CancellationToken cancellationToken);
    }
}