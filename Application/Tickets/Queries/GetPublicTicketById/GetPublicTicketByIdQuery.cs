using MediatR;

namespace Application.Tickets.Queries.GetPublicTicketById
{
    public record GetPublicTicketByIdQuery(int Id) : IRequest<PublicTicketDetailDto>;
}
