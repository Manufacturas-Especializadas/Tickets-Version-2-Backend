using MediatR;

namespace Application.Tickets.Queries.ExportTickets
{
    public record ExportTicketsResponse(byte[] FileContent, string FileName, string ContentType);

    public record ExportTicketsQuery() : IRequest<ExportTicketsResponse>;
}
