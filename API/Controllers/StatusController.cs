using Application.Tickets.Queries.GetStatus;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController(IMediator mediator) : ControllerBase
    {
        [HttpGet("getStatus")]
        public async Task<IActionResult> GetStatus(CancellationToken cancellationToken)
        {
            var query = new GetStatusQuery();

            var classifications = await mediator.Send(query, cancellationToken);

            return Ok(classifications);
        }
    }
}