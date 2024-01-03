using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.UserManagement;

namespace QuoteQuiz.Infrastructure.Database.Users
{
    public static class UserFilterExtensions
    {
        public static IQueryable<User> ApplyFilter(this IQueryable<User> query, UserFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(filter.Login))
            {
                query = query.Where(x => x.Login.Contains(filter.Login));
            }

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }

            return query;
        }
    }
}
