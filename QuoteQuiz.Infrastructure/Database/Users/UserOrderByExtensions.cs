using Microsoft.IdentityModel.Tokens;
using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;
using System.Linq.Expressions;

namespace QuoteQuiz.Infrastructure.Database.Users
{
    public static class UserOrderByExtensions
    {
        public static IQueryable<User> ApplyOrderBy(this IQueryable<User> query, OrderBy by)
        {
            if (by == null || string.IsNullOrEmpty(by.Column))
            {
                // default
                by = new OrderBy();
                by.Column = nameof(User.Id);
                by.Asc = true;
            }

            by.Column.ToLower();

            switch (by.Column)
            {
                case "id":
                    return query.OrderBy(x => x.Id, by.Asc);
                case "name":
                    return query.OrderBy(x => x.Name, by.Asc);
                case "login":
                    return query.OrderBy(x => x.Login, by.Asc);
                case "role":
                    return query.OrderBy(x => x.Role, by.Asc);
            }

            return query;
        }

        private static IQueryable<User> OrderBy<T>(
            this IQueryable<User> query,
            Expression<Func<User, T>> expression,
            bool asc)
        {
            if (asc)
            {
                return query.OrderBy(expression);
            }
            else
            {
                return query.OrderByDescending(expression);
            }
        }
    }
}
