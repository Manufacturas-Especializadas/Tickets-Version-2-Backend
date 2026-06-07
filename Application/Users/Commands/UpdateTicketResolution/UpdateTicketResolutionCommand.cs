using MediatR;

namespace Application.Users.Commands.UpdateTicketResolution
{
    public record UpdateTicketResolutionCommand(
         int TicketId,
         int ResolvingUserId,
         int NewStatusId,
         int? ClassificationId,
         string? Solution
     ) : IRequest<Unit>;
}