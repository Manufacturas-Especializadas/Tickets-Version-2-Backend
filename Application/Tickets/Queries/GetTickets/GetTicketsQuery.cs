using MediatR;

namespace Application.Tickets.Queries.GetTickets
{
    public record GetTicketsQuery() : IRequest<List<TicketListDto>>;
}