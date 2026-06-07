using Application.Common.Interfaces;
using Application.Tickets.Commands.CreateTicket;
using Application.Tickets.Queries.GetTickets;
using Application.Users.Commands.UpdateTicketResolution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("getAll")]
        public async Task<IActionResult> GetTickets(CancellationToken cancellationToken)
        {
            var query = new GetTicketsQuery();

            var tickets = await mediator.Send(query, cancellationToken);

            return Ok(tickets);
        }

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

        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> ResolveTicket(int id, [FromBody] ResolveTicketRequest request)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int resolvingUserId))
                {
                    return Unauthorized(new { error = "Token inválido o no contiene el ID del usuario." });
                }

                var command = new UpdateTicketResolutionCommand(
                    TicketId: id,
                    ResolvingUserId: resolvingUserId,
                    NewStatusId: request.StatusId,
                    ClassificationId: request.ClassificationId,
                    Solution: request.Solution
                );

                await mediator.Send(command);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
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

        public record ResolveTicketRequest(
            int StatusId,
            int? ClassificationId,
            string? Solution
        );
    }
}