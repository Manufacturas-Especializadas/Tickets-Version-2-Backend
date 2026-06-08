using MediatR;

namespace Application.Tickets.Commands.DeleteTicket
{
    public record DeleteTicketCommand(int Id) : IRequest<Unit>;
}