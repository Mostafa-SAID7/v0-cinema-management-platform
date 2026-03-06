using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Application.DTOs.Requests.Users
{
    /// <summary>
    /// Request DTO for user login
    /// </summary>
    public class LoginRequest
    {
        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "The {0} field is required.")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
