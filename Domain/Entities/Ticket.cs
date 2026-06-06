namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        public int? StatusId { get; set; }

        public int? UserId { get; set; }

        public int? ClassificationId { get; set; }

        public required string Name { get; set; }

        public string? Department { get; set; }

        public string? Affair { get; set; }

        public string? ProblemDescription { get; set; }

        public string? Solution { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? ResolutionDate { get; set; }

        public Category? Category { get; set; }

        public Status? Status { get; set; }

        public User? User { get; set; }

        public Classification? Classification { get; set; }

        public ICollection<TicketAttachment> Attachments { get; set; } = new List<TicketAttachment>();
    }
}