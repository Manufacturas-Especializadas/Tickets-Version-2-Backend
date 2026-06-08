using Application.Common.Interfaces;
using Application.Tickets.Commands.CreateTicket;
using Application.Tickets.Commands.DeleteTicket;
using Application.Tickets.Commands.UpdateTicketResolution;
using Application.Tickets.Queries.ExportTickets;
using Application.Tickets.Queries.GetCategorys;
using Application.Tickets.Queries.GetTicketById;
using Application.Tickets.Queries.GetTickets;
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

        [HttpGet("getCategory")]
        public async Task<IActionResult> GetCategory(CancellationToken cancellationToken)
        {
            var query = new GetCategoryQuery();

            var classifications = await mediator.Send(query, cancellationToken);

            return Ok(classifications);
        }

        [HttpGet("DownloadReport")]
        public async Task<IActionResult> DownloadReport(CancellationToken cancellationToken)
        {
            var query = new ExportTicketsQuery();
            var response = await mediator.Send(query, cancellationToken);

            return File(response.FileContent, response.ContentType, response.FileName);
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetTicketById(int id, CancellationToken cancellationToken)
        {
            try
            {
                var query = new GetTicketByIdQuery(id);
                var ticketDetail = await mediator.Send(query, cancellationToken);

                return Ok(ticketDetail);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
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

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTicket(int id, CancellationToken cancellationToken)
        {
            try
            {
                var command = new DeleteTicketCommand(id);
                await mediator.Send(command, cancellationToken);

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