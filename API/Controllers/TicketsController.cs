using Application.Tickets.Commands.CreateTicket;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController(IMediator mediator) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateTicket([FromForm] CreateTicketRequest request, CancellationToken cancellationToken)
        {
            var attachments = request.Files?.Select(f => new FileUploadDto(
                f.FileName,
                f.ContentType,
                f.OpenReadStream()
            )).ToList();

            var command = new CreateTicketCommand(
                request.Name,
                request.Department,
                request.Affair,
                request.ProblemDescription,
                request.CategoryId,
                request.UserId,
                attachments
            );

            var ticketId = await mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(CreateTicket), new { id = ticketId }, new { TicketId = ticketId });
        }

        public record CreateTicketRequest(
            string Name,
            string? Department,
            string? Affair,
            string? ProblemDescription,
            int? CategoryId,
            int? UserId,
            IFormFileCollection? Files
        );
    }
}