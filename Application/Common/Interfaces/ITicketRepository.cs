using Domain.Entities;

namespace Application.Common.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllWithDetailsAsync(CancellationToken cancellationToken);

        Task<int> AddAsync(Ticket ticket, CancellationToken cancellationToken);
    }
}