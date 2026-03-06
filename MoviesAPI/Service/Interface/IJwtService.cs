using MoviesAPI.Domain.Entities.Users;
using System.Security.Claims;

namespace MoviesAPI.Service.Interface
{
    public interface IJwtService
    {
        Task<string> GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
