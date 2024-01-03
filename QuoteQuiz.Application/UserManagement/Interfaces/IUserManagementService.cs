using QuoteQuiz.Api.Core;
using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Application.UserManagement.Interfaces
{
    public interface IUserManagementService
    {
        Task<IList<User>> GetUsersAsync(
            UserFilter? filter = null,
            OrderBy? by = null,
            PaginationSettings? paging = null);
        Task<User?> SaveUserAsync(string? login, string? name, string? password);
        Task<User?> SaveUserAsync(
                    int userId,
                    string name,
                    RoleEnum role,
                    int currentUserId);
        Task<bool> DeleteUserAsync(int userId, int currentUserId);
        Task<bool> DisableUserAsync(int userId, int currentUserId);
    }
}
