namespace MoviesAPI.Application.DTOs.Responses.Users
{
    /// <summary>
    /// Response DTO for user details
    /// </summary>
    public class UserResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Response DTO for authentication
    /// </summary>
    public class AuthenticationResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserResponse User { get; set; }
    }

    /// <summary>
    /// Simplified user response for lists
    /// </summary>
    public class UserSummaryResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// Response for user profile
    /// </summary>
    public class UserProfileResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public int TotalBookings { get; set; }
        public int UpcomingMovies { get; set; }
        public DateTime MemberSince { get; set; }
    }
}
