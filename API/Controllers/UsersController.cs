using Application.Users.Commands.CreateUser;
using Application.Users.Commands.DeleteUser;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
        {
            try
            {
                var command = new CreateUserCommand(
                    request.Name,
                    request.PayRollNumber,
                    request.RolId,
                    request.Password
                );

                var userId = await mediator.Send(command);

                return CreatedAtAction(nameof(RegisterUser), new { id = userId }, new { UserId = userId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await mediator.Send(new DeleteUserCommand(id));
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }
    }
}

public record RegisterUserRequest(string Name, int PayRollNumber, int RolId, string Password);