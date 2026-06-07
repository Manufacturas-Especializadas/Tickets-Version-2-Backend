namespace Application.Auth.Queries.Login
{
    public record AuthResponse(
        int UserId,
        string Name,
        string Role,
        string Token
    );
}
