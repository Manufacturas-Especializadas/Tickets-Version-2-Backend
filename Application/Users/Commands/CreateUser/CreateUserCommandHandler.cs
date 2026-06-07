using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler(
        IApplicationDbContext dbContext,
        IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, int>
    {
        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var exists = await dbContext.Users.AnyAsync(u => u.PayRollNumber == request.PayRollNumber, cancellationToken);
            if (exists)
            {
                throw new InvalidOperationException($"El número de nómina {request.PayRollNumber} ya está registrado.");
            }

            var roleExists = await dbContext.Roles.AnyAsync(r => r.Id == request.RolId, cancellationToken);
            if (!roleExists)
            {
                throw new KeyNotFoundException($"El rol con Id {request.RolId} no existe.");
            }

            var hashedPassword = passwordHasher.Hash(request.Password);

            var user = new User
            {
                Name = request.Name,
                PayRollNumber = request.PayRollNumber,
                RolId = request.RolId,
                PasswordHash = hashedPassword
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }
    }
}