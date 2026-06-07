using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Application.Tickets.Queries.GetStatus
{
    public class GetStatusQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetStatusQuery, List<StatusDto>>
    {
        public async Task<List<StatusDto>> Handle(GetStatusQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Statuses
                .AsNoTracking()
                .Select(c => new StatusDto(c.Id, c.Name))
                .ToListAsync(cancellationToken);
        }
    }
}