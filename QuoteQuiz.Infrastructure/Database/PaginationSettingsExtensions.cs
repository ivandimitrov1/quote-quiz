using QuoteQuiz.Api.Core;

namespace QuoteQuiz.Infrastructure.Database
{
    public static class PaginationSettingsExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PaginationSettings settings)
        {
            if (settings == null)
            {
                return query;
            }

            if (settings.SkipCount.HasValue)
            {
                query = query.Skip(settings.SkipCount.Value);
            }

            if (settings.TakeCount.HasValue)
            {
                query = query.Take(settings.TakeCount.Value);
            }

            return query;
        }
    }
}
