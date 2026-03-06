using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Domain.Entities.Users;
using MoviesAPI.Service.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Users;
using MoviesAPI.Application.DTOs.Responses.Users;
using MoviesAPI.Application.DTOs.Requests.Email;
using AutoMapper;
using FluentValidation;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterUserRequest> _registerValidator;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService, 
            IJwtService jwtService,
            IMapper mapper,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterUserRequest> registerValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _jwtService = jwtService;
            _mapper = mapper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(BaseResponse<AuthenticationResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var validationResult = await _loginValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
                return Unauthorized(BaseResponse<object>.Failure("Invalid username or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
            
            if (result.IsLockedOut)
                return Unauthorized(BaseResponse<object>.Failure("Account is locked out. Please try again later."));

            if (!result.Succeeded)
                return Unauthorized(BaseResponse<object>.Failure("Invalid username or password"));

            // Update user activity
            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var token = await _jwtService.GenerateToken(user);

            var response = new AuthenticationResponse
            {
                Token = token,
                User = _mapper.Map<UserResponse>(user),
                ExpiresAt = DateTime.UtcNow.AddHours(24)
            };

            return Ok(BaseResponse<AuthenticationResponse>.Success(response, "Login successful"));
        }

        [HttpPost("Logout")]
        [Authorize]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<IActionResult> Logout()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(BaseResponse<object>.Failure("Invalid user"));

            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                user.IsActive = false;
                user.UpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }

            await _signInManager.SignOutAsync();

            return Ok(BaseResponse<object>.Success(new { username }, "Logout successful"));
        }

        [HttpPost("Register")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 409)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var validationResult = await _registerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingUserByEmail != null)
                return Conflict(BaseResponse<object>.Failure("Email already registered"));

            var existingUserByUsername = await _userManager.FindByNameAsync(request.Username);
            if (existingUserByUsername != null)
                return Conflict(BaseResponse<object>.Failure("Username already exists"));

            try
            {
                var user = _mapper.Map<User>(request);
                
                var result = await _userManager.CreateAsync(user, request.Password);
                
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(BaseResponse<object>.Failure(errors));
                }

                // Assign default role
                await _userManager.AddToRoleAsync(user, "User");

                return Ok(BaseResponse<object>.Success(new { userId = user.Id }, "Registration successful! You can now log in."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponse<object>.Failure($"Error creating user: {ex.Message}"));
            }
        }

        [HttpPost("ForgotPassword")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(BaseResponse<object>.Failure("Email is required"));

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user doesn't exist
                return Ok(BaseResponse<object>.Success(null, "If this email exists, a reset link has been sent."));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"{Request.Scheme}://{Request.Host}/api/Account/ResetPassword?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(request.Email)}";

            var emailMessage = new EmailMessageRequest
            {
                MailTo = request.Email,
                Subject = "Reset Your Password",
                Content = $@"
            <html>
            <body>
            <p>Hi {user.Name},</p>
            <p>Click the link below to reset your password:</p>
            <p><a href='{resetLink}' style='background-color:#4CAF50;color:white;padding:10px 20px;text-decoration:none;border-radius:5px;'>Reset Password</a></p>
            <p>If you did not request this, please ignore this email.</p>
            </body>
            </html>"
            };

            await _emailService.SendEmailAsync(emailMessage);

            return Ok(BaseResponse<object>.Success(null, "If this email exists, a reset link has been sent."));
        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(BaseResponse<object>.Failure("Email is required"));

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(BaseResponse<object>.Failure("Invalid request"));

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            return Ok(BaseResponse<object>.Success(null, "Password successfully reset. You can now log in."));
        }

        [HttpPost("ConfirmEmail")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return BadRequest(BaseResponse<object>.Failure("Invalid request"));

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return BadRequest(BaseResponse<object>.Failure("User not found"));

            var result = await _userManager.ConfirmEmailAsync(user, token);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            return Ok(BaseResponse<object>.Success(null, "Email confirmed successfully!"));
        }
    }
}
