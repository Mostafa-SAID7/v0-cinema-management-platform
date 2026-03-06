using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Data;
using MoviesAPI.Domain.Entities.Users;
using MoviesAPI.Repositories.Interface;

namespace MoviesAPI.Repositories.Implementation
{
    /// <summary>
    /// Repository implementation for User entity using ASP.NET Core Identity
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userManager.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            // Note: Password should be hashed using UserManager.CreateAsync(user, password)
            // This method is for internal use only
            await _context.Users.AddAsync(user);
            return user;
        }

        public Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public Task DeleteUserAsync(User user)
        {
            // Soft delete
            user.SoftDelete();
            _context.Users.Update(user);
            return Task.CompletedTask;
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null;
        }

        public async Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return null;

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
                return null;

            user.IsActive = true;
            user.UpdatedAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return user;
        }

        public async Task UpdateUserPasswordAsync(Guid userId, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, newPassword);
                user.UpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task UpdateUserRoleAsync(Guid userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                // Remove existing roles
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                
                // Add new role
                await _userManager.AddToRoleAsync(user, role);
                
                user.IsAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
                user.UpdatedAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
