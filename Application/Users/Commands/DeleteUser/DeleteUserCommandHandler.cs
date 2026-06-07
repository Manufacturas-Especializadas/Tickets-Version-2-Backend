using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Application.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler(IApplicationDbContext dbContext) : IRequestHandler<DeleteUserCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users
                .Include(u => u.Tickets)
                .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user is null)
            {
                throw new KeyNotFoundException($"El usuario con Id {request.Id} no fue encontrado.");
            }

            if (user.Tickets.Any())
            {
                throw new InvalidOperationException("No se puede eliminar el usuario porque tiene tickets asignados en el historial.");
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}