using Application.Tickets.Queries.GetClassifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassificationsController(IMediator mediator) : ControllerBase
    {
        [HttpGet("getClassifications")]
        public async Task<IActionResult> GetClassifications(CancellationToken cancellationToken)
        {
            var query = new GetClassificationsQuery();
            var classifications = await mediator.Send(query, cancellationToken);

            return Ok(classifications);
        }
    }
}
