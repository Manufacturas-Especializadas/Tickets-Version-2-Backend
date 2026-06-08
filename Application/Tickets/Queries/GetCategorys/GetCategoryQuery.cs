using MediatR;

namespace Application.Tickets.Queries.GetCategorys
{
    public record GetCategoryQuery() : IRequest<List<CategoryDto>>;
}
