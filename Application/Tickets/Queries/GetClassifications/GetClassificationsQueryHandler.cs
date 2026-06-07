using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Application.Tickets.Queries.GetClassifications
{
    public class GetClassificationsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetClassificationsQuery, List<ClassificationDto>>
    {
        public async Task<List<ClassificationDto>> Handle(GetClassificationsQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Classifications
                .AsNoTracking()
                .Select(c => new ClassificationDto(c.Id, c.Name))
                .ToListAsync(cancellationToken);
        }
    }
}