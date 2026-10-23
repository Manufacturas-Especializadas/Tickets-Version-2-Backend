namespace Application.Tickets.Queries.GetPublicTickets
{
    public record PublicTicketListDto(
        int Id,
        string Name,
        string? Department,
        string? Affair,
        string Category,
        string Status,
        string? PublicComment,
        DateTime RegistrationDate,
        DateTime? ResolutionDate
    )
    {
        public string Folio => $"TK-{Id:D6}";
    }
}
