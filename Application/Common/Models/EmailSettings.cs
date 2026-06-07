namespace Application.Common.Models
{
    public class EmailSettings
    {
        public required string SenderName { get; set; }

        public required string SenderEmail { get; set; }

        public required string Host { get; set; }

        public int Port { get; set; }

        public bool UseSSL { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}