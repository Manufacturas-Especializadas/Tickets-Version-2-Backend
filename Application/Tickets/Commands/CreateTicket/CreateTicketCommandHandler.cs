using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Tickets.Commands.CreateTicket
{
    public class CreateTicketCommandHandler(
        ITicketRepository ticketRepository,
        IFileStorageService fileStorageService) : IRequestHandler<CreateTicketCommand, int>
    {
        public async Task<int> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");

            DateTime nowInMexico = TimeZoneInfo.ConvertTime(DateTime.UtcNow, mexicoTimeZone);

            var ticket = new Ticket
            {
                Name = request.Name,
                Department = request.Department,
                Affair = request.Affair,
                ProblemDescription = request.ProblemDescription,
                CategoryId = request.CategoryId,
                UserId = request.UserId,
                RegistrationDate = nowInMexico
            };

            if (request.Attachments is not null && request.Attachments.Any())
            {
                foreach (var file in request.Attachments)
                {
                    var fileUrl = await fileStorageService.UploadFileAsync(file, cancellationToken);

                    ticket.Attachments.Add(new TicketAttachment
                    {
                        FileName = file.FileName,
                        FileUrl = fileUrl
                    });
                }
            }

            await ticketRepository.AddAsync(ticket, cancellationToken);

            return ticket.Id;
        }
    }
}