using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;


namespace Application.Tickets.Commands.DeleteTicket
{
    public class DeleteTicketCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteTicketCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await dbContext.Tickets
                .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

            if (ticket is null)
            {
                throw new KeyNotFoundException($"El ticket con Id {request.Id} no existe.");
            }

            dbContext.Tickets.Remove(ticket);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}