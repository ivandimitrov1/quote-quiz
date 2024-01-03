using QuoteQuiz.Api.Core;
using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.UserManagement.Interfaces;

namespace QuoteQuiz.Application.UserManagement
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAsyncDbContext _asyncDbContext;

        public UserManagementService(
            IUserRepository userRepository,
            IAsyncDbContext asyncDbContext)
        {
            _userRepository = userRepository;
            _asyncDbContext = asyncDbContext;
        }

        public async Task<IList<User>> GetUsersAsync(
            UserFilter? filter = null,
            OrderBy? by = null,
            PaginationSettings? paging = null)
        {
            return await _userRepository.GetUsersAsync(filter, by, paging);
        }

        public async Task<User?> SaveUserAsync(
            string? login,
            string? password,
            string? name = null)
        {
            var user = new User(login, password, name);

            var existingUser = await _userRepository.GetUserByLoginAsync(user.Login);
            if (existingUser != null)
            {
                throw new QuoteQuizApplicationException("Username already exists.");
            }

            _userRepository.AddUser(user);
            await _asyncDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> SaveUserAsync(
            int userId,
            string name,
            RoleEnum role,
            int currentUserId)
        {
            var existingUser = await _userRepository.GetAsync(userId);

            existingUser.Name = name;
            existingUser.SetRole(role, currentUserId);

            await _asyncDbContext.SaveChangesAsync();
            return existingUser;
        }

        public async Task<bool> DeleteUserAsync(int userId, int currentUserId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user != null)
            {
                user.Delete(currentUserId);
                await _asyncDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> DisableUserAsync(int userId, int currentUserId)
        {
            var user = await _userRepository.GetAsync(userId);
            if (user != null)
            {
                user.SetRole(RoleEnum.UserReadOnly, currentUserId);
                await _asyncDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
