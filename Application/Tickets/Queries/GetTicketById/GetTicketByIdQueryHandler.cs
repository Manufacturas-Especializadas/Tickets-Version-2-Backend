using Application.Common.Interfaces;
using MediatR;

namespace Application.Tickets.Queries.GetTicketById
{
    public class GetTicketByIdQueryHandler(ITicketRepository ticketRepository)
        : IRequestHandler<GetTicketByIdQuery, TicketDetailDto>
    {
        public async Task<TicketDetailDto> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var ticket = await ticketRepository.GetByIdWithDetailsAsync(request.Id, cancellationToken);

            if (ticket is null)
            {
                throw new KeyNotFoundException($"El ticket con Id {request.Id} no fue encontrado.");
            }

            return new TicketDetailDto(
                Id: ticket.Id,
                Name: ticket.Name,
                Department: ticket.Department,
                Affair: ticket.Affair,
                ProblemDescription: ticket.ProblemDescription,
                StatusId: ticket.StatusId ?? 1,
                ClassificationId: ticket.ClassificationId,
                Solution: ticket.Solution,
                Category: ticket.Category?.Name ?? "Sin categoría",
                Status: ticket.Status?.Name ?? "Desconocido",
                Classification: ticket.Classification?.Name,
                RegistrationDate: ticket.RegistrationDate,
                ResolutionDate: ticket.ResolutionDate,
                Attachments: ticket.Attachments.Select(a => new AttachmentDto(
                    Id: a.Id,
                    FileName: a.FileName,
                    FileUrl: a.FileUrl
                )).ToList()
            );
        }
    }
}