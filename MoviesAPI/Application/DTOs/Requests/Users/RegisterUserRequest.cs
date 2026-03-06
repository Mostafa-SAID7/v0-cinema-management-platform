using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Application.DTOs.Requests.Users
{
    /// <summary>
    /// Request DTO for user registration
    /// </summary>
    public class RegisterUserRequest
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
