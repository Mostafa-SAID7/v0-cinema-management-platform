using MoviesAPI.Domain.Entities.Users;

namespace MoviesAPI.Repositories.Interface
{
    /// <summary>
    /// Repository interface for User entity operations
    /// </summary>
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<bool> IsEmailTakenAsync(string email);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<User?> GetUserByUsernameAndPasswordAsync(string username, string password);
        Task UpdateUserPasswordAsync(Guid userId, string newPassword);
        Task UpdateUserRoleAsync(Guid userId, string role);
    }
}
