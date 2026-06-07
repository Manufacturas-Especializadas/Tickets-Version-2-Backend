using MediatR;

namespace Application.Tickets.Queries.GetTicketById
{
    public record GetTicketByIdQuery(int Id) : IRequest<TicketDetailDto>;
}