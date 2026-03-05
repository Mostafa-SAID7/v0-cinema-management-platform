using MoviesAPI.Models.System;
using System.Security.Claims;

namespace MoviesAPI.Service.Interface
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        ClaimsPrincipal? ValidateToken(string token);
    }
}
