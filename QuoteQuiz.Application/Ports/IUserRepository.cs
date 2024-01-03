using QuoteQuiz.Api.Core;
using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.UserManagement;

namespace QuoteQuiz.Application.Ports
{
    public interface IUserRepository
    {
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
        Task<User?> GetUserByLoginAsync(string login);
        Task<IList<User>> GetUsersAsync(
            UserFilter? filter = null,
            OrderBy? by = null,
            PaginationSettings? paging = null);
        Task<User> GetAsync(int userId);
        void AddUser(User user);
    }
}
