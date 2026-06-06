namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public int PayRollNumber { get; set; }

        public int RolId { get; set; }

        public required string PasswordHash { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }

        public Role? Role { get; set; }

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}