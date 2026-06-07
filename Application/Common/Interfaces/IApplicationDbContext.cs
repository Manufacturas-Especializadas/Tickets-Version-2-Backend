using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Category> Categories { get; }

        DbSet<Status> Statuses { get; }

        DbSet<Classification> Classifications { get; }

        DbSet<Role> Roles { get; }

        DbSet<User> Users { get; }

        DbSet<Ticket> Tickets { get; }

        DbSet<TicketAttachment> TicketAttachments { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
