namespace MoviesAPI.Application.DTOs.Responses.Users
{
    /// <summary>
    /// Response DTO for user summary in lists
    /// </summary>
    public class UserSummaryResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
    }
}
