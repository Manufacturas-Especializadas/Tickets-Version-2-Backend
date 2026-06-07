using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Application.Users.Commands.UpdateTicketResolution
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


            ticket.UserId = request.ResolvingUserId;
            ticket.StatusId = request.NewStatusId;

            if (!string.IsNullOrWhiteSpace(request.Solution))
            {
                ticket.Solution = request.Solution;
            }

            if (request.ClassificationId.HasValue)
            {
                ticket.ClassificationId = request.ClassificationId.Value;
            }

            if (request.NewStatusId == 3 && ticket.ResolutionDate is null)
            {
                ticket.ResolutionDate = DateTime.Now;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}