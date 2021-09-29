using BadOrder.Library.Models.Users;

namespace BadOrder.Library.Services
{
    public interface IAuthService
    {
        string AdminRole { get; }
        string UserRole { get; }

        string GenerateJwtToken(User user);
        string HashPassword(string password);
        bool IsAuthRole(string role);
        bool VerifyUserPassword(string loginPassword, string storedPassword);
    }
}