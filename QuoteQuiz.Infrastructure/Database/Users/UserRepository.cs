using Microsoft.EntityFrameworkCore;
using QuoteQuiz.Api.Core;
using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.UserManagement;

namespace QuoteQuiz.Infrastructure.Database.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<User?> GetAsync(int userId)
        {
            // TO:DO
            // add changedate to the user 
            // for optimistic concurrency
            if (userId < 0)
            {
                return null;
            }

            var user = await _applicationDbContext.Users.FindAsync(userId);
            if (user == null) 
            {
                return null;
            }

            await _applicationDbContext.Entry(user).Collection(i => i.RefreshTokens).LoadAsync();
            await _applicationDbContext.Entry(user).Collection(i => i.Answers).LoadAsync();
            return user;
        }

        public async Task<IList<User>> GetUsersAsync(
            UserFilter? filter = null,
            OrderBy? by = null,
            PaginationSettings? paging = null)
        {
            return 
                await _applicationDbContext
                        .Users
                        .Include(x => x.Answers)
                        .Where(x => !x.Deleted)
                        .ApplyFilter(filter)
                        .ApplyOrderBy(by)
                        .ApplyPaging(paging)
                        .ToListAsync();
        }

        public void AddUser(User user)
        {
            _applicationDbContext.Users.Add(user);
        }

        public async Task<User?> GetUserByLoginAsync(string? login)
        {
            return await _applicationDbContext
                            .Users.Where(x => x.Login == login)
                            .Include(x => x.RefreshTokens)
                            .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string tokenHash)
        {
            return await _applicationDbContext
                            .Users
                            .Include(x => x.RefreshTokens)
                            .Where(x => x.RefreshTokens.Any(t => t.TokenHash == tokenHash))
                            .FirstOrDefaultAsync();
        }
    }
}
