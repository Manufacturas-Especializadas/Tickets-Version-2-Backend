using Application.Common.Interfaces;
using MediatR;

namespace Application.Tickets.Queries.GetTickets
{
    public class GetTicketsQueryHandler(ITicketRepository ticketRepository)
    : IRequestHandler<GetTicketsQuery, List<TicketListDto>>
    {
        public async Task<List<TicketListDto>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
        {
            var tickets = await ticketRepository.GetAllWithDetailsAsync(cancellationToken);

            return tickets.Select(t => new TicketListDto(
                Id: t.Id.ToString(),
                Nombre: t.Name,
                Categoria: t.Category?.Name ?? "Sin categoría",
                Estatus: t.Status?.Name ?? "Pendiente"
            )).ToList();
        }
    }
}