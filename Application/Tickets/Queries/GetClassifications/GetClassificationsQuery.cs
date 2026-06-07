using MediatR;

namespace Application.Tickets.Queries.GetClassifications
{
    public record GetClassificationsQuery() : IRequest<List<ClassificationDto>>;
}
