namespace Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendNewTicketNotificationAsync(string name, string? department, string? affair, string categoryName);
    }
}