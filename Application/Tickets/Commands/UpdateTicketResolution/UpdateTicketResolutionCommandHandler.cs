using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Application.Tickets.Commands.UpdateTicketResolution
{
    public class UpdateTicketResolutionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateTicketResolutionCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateTicketResolutionCommand request, CancellationToken cancellationToken)
        {
            var ticket = await dbContext.Tickets
                .FirstOrDefaultAsync(t => t.Id == request.TicketId, cancellationToken);

            if (ticket is null)
            {
                throw new KeyNotFoundException($"El ticket con Id {request.TicketId} no existe.");
            }

            ticket.StatusId = request.NewStatusId;

            ticket.Solution = request.Solution;
            ticket.ClassificationId = request.ClassificationId;

            if (request.NewStatusId == 3)
            {
                if (ticket.ResolutionDate is null)
                {
                    ticket.ResolutionDate = DateTime.Now;
                }

                ticket.UserId = request.ResolvingUserId;
            }
            else
            {
                ticket.ResolutionDate = null;
                ticket.UserId = null;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}