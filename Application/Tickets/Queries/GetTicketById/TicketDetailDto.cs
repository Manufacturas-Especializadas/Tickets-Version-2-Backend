namespace Application.Tickets.Queries.GetTicketById
{
    public record TicketDetailDto(
        int Id,
        string Name,
        string? Department,
        string? Affair,
        string? ProblemDescription,
        string? Solution,
        string Category,
        string Status,
        string? Classification,
        DateTime RegistrationDate,
        DateTime? ResolutionDate,
        List<AttachmentDto> Attachments
    );

    public record AttachmentDto(
        int Id,
        string? FileName,
        string FileUrl
    );
}