using System.ComponentModel.DataAnnotations;

namespace MoviesAPI.Application.DTOs.Requests.Users
{
    /// <summary>
    /// Request DTO for updating user profile
    /// </summary>
    public class UpdateUserProfileRequest
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; }
        
        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; }
    }

    /// <summary>
    /// Request DTO for changing password
    /// </summary>
    public class ChangePasswordRequest
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }
        
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Required]
        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Request DTO for updating user security settings
    /// </summary>
    public class UpdateSecuritySettingsRequest
    {
        public bool TwoFactorEnabled { get; set; }
    }

    /// <summary>
    /// Request DTO for updating user preferences
    /// </summary>
    public class UpdateUserPreferencesRequest
    {
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public string PreferredLanguage { get; set; }
        public string PreferredGenres { get; set; }
    }
}
