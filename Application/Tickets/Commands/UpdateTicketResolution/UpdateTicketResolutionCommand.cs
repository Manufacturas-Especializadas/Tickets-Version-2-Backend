using MediatR;

namespace Application.Tickets.Commands.UpdateTicketResolution
{
    public record UpdateTicketResolutionCommand(
         int TicketId,
         int ResolvingUserId,
         int NewStatusId,
         int? ClassificationId,
         string? Solution,
         string? PublicComment = null
     ) : IRequest<Unit>;
}
