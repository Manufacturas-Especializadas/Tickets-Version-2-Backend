using MediatR;

namespace Application.Tickets.Queries.GetStatus
{
    public record GetStatusQuery(): IRequest<List<StatusDto>>;
}
