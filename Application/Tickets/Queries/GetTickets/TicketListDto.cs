namespace Application.Tickets.Queries.GetTickets;

public record TicketListDto(
    string Id,
    string Nombre,
    string Categoria,
    string Estatus
);