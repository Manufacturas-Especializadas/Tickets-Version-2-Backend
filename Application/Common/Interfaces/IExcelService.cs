namespace Application.Common.Interfaces
{
    public interface IExcelService
    {
        byte[] GenerateTicketsReport(IEnumerable<TicketReportModel> tickets);
    }
}

public record TicketReportModel(
    int Id,
    string Name,
    string Category,
    string Status,
    DateTime RegistrationDate,
    DateTime? ResolutionDate,
    string ResolvedBy
);