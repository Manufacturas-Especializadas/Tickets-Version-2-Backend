using MediatR;

namespace Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(
        string Name,
        int PayRollNumber,
        int RolId,
        string Password
    ) : IRequest<int>;
}