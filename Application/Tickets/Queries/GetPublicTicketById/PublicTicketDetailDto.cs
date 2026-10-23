namespace Application.Tickets.Queries.GetPublicTicketById
{
    public record PublicTicketDetailDto(
        int Id,
        string Name,
        string? Department,
        string? Affair,
        string? ProblemDescription,
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
