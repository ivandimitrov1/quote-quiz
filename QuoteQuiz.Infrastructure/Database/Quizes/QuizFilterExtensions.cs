using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Quizes;

namespace QuoteQuiz.Infrastructure.Database.Quizes
{
    public static class QuizFilterExtensions
    {
        public static IQueryable<Quiz> ApplyFilter(this IQueryable<Quiz> query, QuizFilter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.Published.HasValue)
            {
                query = query.Where(x => x.Published == filter.Published);
            }

            return query;
        }
    }
}
