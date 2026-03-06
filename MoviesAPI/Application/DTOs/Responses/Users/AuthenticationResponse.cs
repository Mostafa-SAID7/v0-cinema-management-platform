namespace MoviesAPI.Application.DTOs.Responses.Users
{
    /// <summary>
    /// Response DTO for authentication with token
    /// </summary>
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public UserResponse User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
