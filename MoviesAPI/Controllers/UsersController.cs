using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Domain.Entities.Users;
using MoviesAPI.Data;
using MoviesAPI.Application.DTOs.Common;
using MoviesAPI.Application.DTOs.Requests.Users;
using MoviesAPI.Application.DTOs.Responses.Users;
using AutoMapper;
using FluentValidation;
using System.Security.Claims;

namespace MoviesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IValidator<UpdateUserProfileRequest> _updateValidator;

        public UsersController(
            UserManager<User> userManager,
            ApplicationDbContext context,
            IMapper mapper,
            IValidator<UpdateUserProfileRequest> updateValidator)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _updateValidator = updateValidator;
        }

        /// <summary>
        /// Get all users (Admin only)
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BaseResponse<List<UserSummaryResponse>>), 200)]
        public async Task<ActionResult<BaseResponse<List<UserSummaryResponse>>>> GetAllUsers()
        {
            var users = await _userManager.Users
                .Where(u => !u.IsDeleted)
                .ToListAsync();
            
            var response = _mapper.Map<List<UserSummaryResponse>>(users);
            return Ok(BaseResponse<List<UserSummaryResponse>>.Success(response));
        }

        /// <summary>
        /// Get user by ID (Admin only or own profile)
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<UserResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        [ProducesResponseType(typeof(BaseResponse<object>), 403)]
        public async Task<ActionResult<BaseResponse<UserResponse>>> GetUser(Guid id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && currentUserId != id.ToString())
                return Forbid();

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null || user.IsDeleted)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            var response = _mapper.Map<UserResponse>(user);
            return Ok(BaseResponse<UserResponse>.Success(response));
        }

        /// <summary>
        /// Get current user profile with statistics
        /// </summary>
        [HttpGet("profile")]
        [ProducesResponseType(typeof(BaseResponse<UserProfileResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<ActionResult<BaseResponse<UserProfileResponse>>> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(BaseResponse<object>.Failure("User not authenticated"));

            var user = await _userManager.Users
                .Include(u => u.Tickets)
                .Include(u => u.MovieRatings)
                .FirstOrDefaultAsync(u => u.Id == Guid.Parse(userId));

            if (user == null || user.IsDeleted)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            var profile = _mapper.Map<UserProfileResponse>(user);
            
            // Calculate statistics
            profile.TotalBookings = user.Tickets?.Count ?? 0;
            profile.UpcomingMovies = user.Tickets?
                .Count(t => t.WatchDateTime > DateTime.UtcNow) ?? 0;
            profile.TotalRatings = user.MovieRatings?.Count ?? 0;
            profile.TotalSpent = user.Tickets?.Sum(t => t.Price) ?? 0;

            // Calculate favorite genre
            if (user.MovieRatings != null && user.MovieRatings.Any())
            {
                var genreStats = await _context.MovieRatings
                    .Where(mr => mr.UserId == user.Id)
                    .Include(mr => mr.Movie)
                    .GroupBy(mr => mr.Movie.Genres)
                    .Select(g => new { Genre = g.Key, Count = g.Count() })
                    .OrderByDescending(g => g.Count)
                    .FirstOrDefaultAsync();

                if (genreStats != null)
                {
                    profile.FavoriteGenre = genreStats.Genre;
                    profile.FavoriteGenreCount = genreStats.Count;
                }
            }

            return Ok(BaseResponse<UserProfileResponse>.Success(profile));
        }

        /// <summary>
        /// Update current user profile
        /// </summary>
        [HttpPut("profile")]
        [ProducesResponseType(typeof(BaseResponse<UserProfileUpdateResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<UserProfileUpdateResponse>>> UpdateMyProfile([FromBody] UpdateUserProfileRequest request)
        {
            var validationResult = await _updateValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.IsDeleted)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            // Update user properties
            user.Name = request.Name;
            user.PhoneNumber = request.Phone;
            user.UpdatedAt = DateTime.UtcNow;

            // Check if email changed
            if (user.Email != request.Email)
            {
                var emailExists = await _userManager.FindByEmailAsync(request.Email);
                if (emailExists != null && emailExists.Id != user.Id)
                    return BadRequest(BaseResponse<object>.Failure("Email already in use"));

                user.Email = request.Email;
                user.EmailConfirmed = false; // Require re-confirmation
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            var response = _mapper.Map<UserProfileUpdateResponse>(user);
            response.Message = "Profile updated successfully";

            return Ok(BaseResponse<UserProfileUpdateResponse>.Success(response));
        }

        /// <summary>
        /// Change password for current user
        /// </summary>
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<object>>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.IsDeleted)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Ok(BaseResponse<object>.Success(null, "Password changed successfully"));
        }

        /// <summary>
        /// Get security settings for current user
        /// </summary>
        [HttpGet("security")]
        [ProducesResponseType(typeof(BaseResponse<UserSecurityResponse>), 200)]
        public async Task<ActionResult<BaseResponse<UserSecurityResponse>>> GetSecuritySettings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.IsDeleted)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            var response = _mapper.Map<UserSecurityResponse>(user);
            return Ok(BaseResponse<UserSecurityResponse>.Success(response));
        }

        /// <summary>
        /// Update security settings for current user
        /// </summary>
        [HttpPut("security")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<ActionResult<BaseResponse<object>>> UpdateSecuritySettings([FromBody] UpdateSecuritySettingsRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.IsDeleted)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            var result = await _userManager.SetTwoFactorEnabledAsync(user, request.TwoFactorEnabled);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Ok(BaseResponse<object>.Success(null, "Security settings updated successfully"));
        }

        /// <summary>
        /// Delete current user account (soft delete)
        /// </summary>
        [HttpDelete("profile")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.IsDeleted)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            // Soft delete
            user.SoftDelete(Guid.Parse(userId));
            await _userManager.UpdateAsync(user);

            return Ok(BaseResponse<object>.Success(null, "Account deleted successfully"));
        }

        /// <summary>
        /// Update user role (Admin only)
        /// </summary>
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(BaseResponse<object>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(BaseResponse<object>), 404)]
        public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] UpdateRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null || user.IsDeleted)
                return NotFound(BaseResponse<object>.Failure("User not found"));

            if (string.IsNullOrWhiteSpace(request.Role))
                return BadRequest(BaseResponse<object>.Failure("Role is required"));

            // Remove existing roles
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            // Add new role
            var result = await _userManager.AddToRoleAsync(user, request.Role);
            
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(BaseResponse<object>.Failure(errors));
            }

            user.IsAdmin = request.Role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Ok(BaseResponse<object>.Success(null, "User role updated successfully"));
        }
    }
}
