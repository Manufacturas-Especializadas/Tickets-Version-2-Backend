using Application.Common.Interfaces;
using MediatR;

namespace Application.Tickets.Commands.CreateTicket
{
    public record CreateTicketCommand(
    string Name,
    string? Department,
    string? Affair,
    string? ProblemDescription,
    int? CategoryId,
    int? UserId,
    List<FileUploadDto>? Attachments
) : IRequest<int>;
}