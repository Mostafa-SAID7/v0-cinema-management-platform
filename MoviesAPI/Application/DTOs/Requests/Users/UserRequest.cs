namespace MoviesAPI.Application.DTOs.Requests.Users
{
    /// <summary>
    /// Request DTO for user registration
    /// </summary>
    public class RegisterUserRequest
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Request DTO for user login
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Request DTO for updating user profile
    /// </summary>
    public class UpdateUserProfileRequest
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    /// <summary>
    /// Request DTO for changing password
    /// </summary>
    public class ChangePasswordRequest
    {
        public long UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }

    /// <summary>
    /// Request DTO for updating user role (Admin only)
    /// </summary>
    public class UpdateUserRoleRequest
    {
        public long UserId { get; set; }
        public string Role { get; set; } // Admin, User
    }

    /// <summary>
    /// Request DTO for password reset
    /// </summary>
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
    }

    /// <summary>
    /// Request DTO for resetting password with token
    /// </summary>
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
