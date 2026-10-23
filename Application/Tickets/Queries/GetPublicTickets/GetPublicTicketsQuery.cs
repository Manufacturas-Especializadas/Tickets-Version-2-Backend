using MediatR;

namespace Application.Tickets.Queries.GetPublicTickets
{
    public record GetPublicTicketsQuery() : IRequest<List<PublicTicketListDto>>;
}
