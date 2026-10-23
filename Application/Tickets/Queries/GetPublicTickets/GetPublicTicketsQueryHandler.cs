using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tickets.Queries.GetPublicTickets
{
    public class GetPublicTicketsQueryHandler(IApplicationDbContext dbContext)
        : IRequestHandler<GetPublicTicketsQuery, List<PublicTicketListDto>>
    {
        public async Task<List<PublicTicketListDto>> Handle(GetPublicTicketsQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Tickets
                .AsNoTracking()
                .OrderByDescending(t => t.RegistrationDate)
                .ThenByDescending(t => t.Id)
                .Select(t => new PublicTicketListDto(
                    t.Id,
                    t.Name,
                    t.Department,
                    t.Affair,
                    t.Category != null ? t.Category.Name : "Sin categoría",
                    t.Status != null ? t.Status.Name : "Desconocido",
                    t.PublicComment,
                    t.RegistrationDate,
                    t.ResolutionDate
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
