using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Auth.Queries.Login
{
    public class LoginQueryHandler(
     IApplicationDbContext dbContext,
     IJwtTokenGenerator jwtTokenGenerator,
     IPasswordHasher passwordHasher) : IRequestHandler<LoginQuery, AuthResponse>
    {
        public async Task<AuthResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.PayRollNumber == request.PayRollNumber, cancellationToken);

            if (user is null || !passwordHasher.Verify(user.PasswordHash, request.Password))
            {
                throw new UnauthorizedAccessException("Credenciales inválidas.");
            }

            var token = jwtTokenGenerator.GenerateToken(user);

            return new AuthResponse(
                UserId: user.Id,
                Name: user.Name,
                Role: user.Role?.Name ?? "User",
                Token: token
            );
        }
    }
}
