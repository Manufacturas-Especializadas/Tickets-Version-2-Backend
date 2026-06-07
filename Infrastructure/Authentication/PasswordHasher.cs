using Application.Common.Interfaces;

namespace Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string passwordHash, string inputPassword) => BCrypt.Net.BCrypt.Verify(inputPassword, passwordHash);
    }
}