namespace MoviesAPI.Application.DTOs.Responses.Users
{
    /// <summary>
    /// Response DTO for user details
    /// </summary>
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }

    /// <summary>
    /// Response for detailed user profile with statistics
    /// </summary>
    public class UserProfileResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        
        // Statistics
        public int TotalBookings { get; set; }
        public int UpcomingMovies { get; set; }
        public int TotalRatings { get; set; }
        public decimal TotalSpent { get; set; }
        public int FavoriteGenreCount { get; set; }
        public string FavoriteGenre { get; set; }
        
        // Dates
        public DateTime MemberSince { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Response for user profile update
    /// </summary>
    public class UserProfileUpdateResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Response for user security settings
    /// </summary>
    public class UserSecurityResponse
    {
        public Guid Id { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public DateTime? LastPasswordChangedAt { get; set; }
    }
}
