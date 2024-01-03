using Microsoft.EntityFrameworkCore;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.Quizes;

namespace QuoteQuiz.Infrastructure.Database.Quizes
{
    public class QuizRepository : IQuizRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public QuizRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void AddQuiz(Quiz quiz)
        {
            _applicationDbContext.Quizes.Add(quiz);
        }

        public async Task<IList<Quiz>> GetListAsync(QuizFilter filter)
        {
            return await _applicationDbContext
                .Quizes
                .Include(x => x.Quotes)
                .ApplyFilter(filter)
                .ToListAsync();
        }

        public async Task<Quiz?>? GetAsync(int quizId)
        {
            // TO:DO
            // add base repo for common operations

            // TO:DO
            // add changedate to the quiz 
            // for optimistic concurrency
            if (quizId < 0)
            {
                return null;
            }

            var quiz = await _applicationDbContext.Quizes.FindAsync(quizId);
            if (quiz == null)
            {
                return null;
            }

            await _applicationDbContext.Entry(quiz).Collection(i => i.Quotes).LoadAsync();
            return quiz;
        }
    }
}
