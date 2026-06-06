namespace Domain.Entities
{
    public class TicketAttachment
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        public string? FileName { get; set; }

        public required string FileUrl { get; set; }

        public DateTime UploadedDate { get; set; }

        public Ticket? Ticket { get; set; }
    }
}