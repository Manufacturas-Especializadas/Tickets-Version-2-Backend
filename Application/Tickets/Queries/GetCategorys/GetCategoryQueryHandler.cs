using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace Application.Tickets.Queries.GetCategorys
{
    public class GetCategoryQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCategoryQuery, List<CategoryDto>>
    {
        public async Task<List<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            return await dbContext.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto(c.Id, c.Name))
                .ToListAsync(cancellationToken);
        }
    }
}