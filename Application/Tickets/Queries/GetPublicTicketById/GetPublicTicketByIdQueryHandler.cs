using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Tickets.Queries.GetPublicTicketById
{
    public class GetPublicTicketByIdQueryHandler(IApplicationDbContext dbContext)
        : IRequestHandler<GetPublicTicketByIdQuery, PublicTicketDetailDto>
    {
        public async Task<PublicTicketDetailDto> Handle(GetPublicTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var ticket = await dbContext.Tickets
                .AsNoTracking()
                .Where(t => t.Id == request.Id)
                .Select(t => new PublicTicketDetailDto(
                    t.Id,
                    t.Name,
                    t.Department,
                    t.Affair,
                    t.ProblemDescription,
                    t.Category != null ? t.Category.Name : "Sin categoría",
                    t.Status != null ? t.Status.Name : "Desconocido",
                    t.PublicComment,
                    t.RegistrationDate,
                    t.ResolutionDate
                ))
                .FirstOrDefaultAsync(cancellationToken);

            if (ticket is null)
            {
                throw new KeyNotFoundException($"El ticket con Id {request.Id} no fue encontrado.");
            }

            return ticket;
        }
    }
}
