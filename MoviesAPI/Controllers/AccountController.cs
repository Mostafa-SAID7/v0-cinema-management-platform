using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Models.System;
using MoviesAPI.Repositories.Interface;
using MoviesAPI.Service.Interface;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Users;
using MoviesAPI.Application.DTOs.Responses.Users;
using AutoMapper;
using FluentValidation;
using System.Text;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginRequest> _loginValidator;
        private readonly IValidator<RegisterUserRequest> _registerValidator;

        public AccountController(
            IUserRepository userRepository, 
            IEmailService emailService, 
            IJwtService jwtService,
            IMapper mapper,
            IValidator<LoginRequest> loginValidator,
            IValidator<RegisterUserRequest> registerValidator)
        {
            _userRepository = userRepository;
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

            var user = await _userRepository.GetUserByUsernameAndPassword(request.Username, request.Password);

            if (user == null)
                return Unauthorized(BaseResponse<object>.Failure("Invalid username or password"));

            var token = _jwtService.GenerateToken(user);

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
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> Logout([FromBody] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(BaseResponse<object>.Failure("Username is required"));

            var success = await _userRepository.LogoutUser(username);

            if (!success)
                return NotFound(BaseResponse<object>.Failure("User not found or already logged out"));

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

            if (await _userRepository.IsEmailTakenAsync(request.Email))
                return Conflict(BaseResponse<object>.Failure("Email already registered"));

            var existingUser = await _userRepository.GetUserByUsername(request.Username);
            if (existingUser != null)
                return Conflict(BaseResponse<object>.Failure("Username already exists"));

            try
            {
                var registerRequest = _mapper.Map<RegisterRequest>(request);
                registerRequest.isActive = true;
                registerRequest.EmailConfirmed = true;
                
                var userId = await _userRepository.CreateUserAsync(registerRequest);
                return Ok(BaseResponse<object>.Success(new { userId }, "Registration successful! You can now log in."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, BaseResponse<object>.Failure($"Error creating user: {ex.Message}"));
            }
        }

        public static class TempRegistrationStore
        {
            public static Dictionary<string, RegisterRequest> PendingRegistrations = new();

            public static void Add(string token, RegisterRequest request) => PendingRegistrations[token] = request;

            public static RegisterRequest? Get(string token)
            {
                if (PendingRegistrations.TryGetValue(token, out var request))
                {
                    PendingRegistrations.Remove(token); // remove after retrieval
                    return request;
                }
                return null;
            }
        }

        [HttpGet("ConfirmEmail")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
        {
            var pendingRequest = TempRegistrationStore.Get(token);
            if (pendingRequest == null)
                return BadRequest(BaseResponse<object>.Failure("Invalid or expired token"));

            var user = new User
            {
                Username = pendingRequest.Username,
                Email = pendingRequest.Email,
                Password = pendingRequest.Password,
                IsActive = true
            };

            pendingRequest.isActive = true;
            var userId = await _userRepository.CreateUserAsync(pendingRequest);

            return Ok(BaseResponse<object>.Success(new { userId }, "Email confirmed and user account created! You can now log in."));
        }

        [HttpPost("ForgotPassword")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest(BaseResponse<object>.Failure("Email is required"));

            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return Ok(BaseResponse<object>.Success(null, "If this email exists, a reset link has been sent."));
            }

            var token = Guid.NewGuid().ToString();
            TempResetPasswordStore.Add(token, request.Email);

            var resetLink = $"https://localhost:7268/Account/ResetPassword?token={token}";

            var emailMessage = new EmailMessage
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

        public static class TempResetPasswordStore
        {
            public static Dictionary<string, string> PendingResets = new();

            public static void Add(string token, string email) => PendingResets[token] = email;

            public static string? GetEmail(string token)
            {
                if (PendingResets.TryGetValue(token, out var email))
                {
                    PendingResets.Remove(token); // remove after retrieval
                    return email;
                }
                return null;
            }
        }

        public class ResetPasswordRequest
        {
            public string Token { get; set; }
            public string NewPassword { get; set; }
        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var email = TempResetPasswordStore.GetEmail(request.Token);
            if (email == null)
                return BadRequest(BaseResponse<object>.Failure("Invalid or expired token"));

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return BadRequest(BaseResponse<object>.Failure("User not found"));

            user.Password = request.NewPassword;
            await _userRepository.UpdateUserPasswordAsync(user.Id, request.NewPassword);

            return Ok(BaseResponse<object>.Success(null, "Password successfully reset. You can now log in."));
        }
    }
}
