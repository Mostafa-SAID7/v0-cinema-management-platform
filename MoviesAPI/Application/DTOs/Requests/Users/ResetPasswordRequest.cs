using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Application.DTOs.Requests.Users
{
    /// <summary>
    /// Request DTO for password reset
    /// </summary>
    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Token { get; set; }
        
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
