using MediatR;

namespace Application.Auth.Queries.Login
{
    public record LoginQuery(int PayRollNumber, string Password) : IRequest<AuthResponse>;
}
