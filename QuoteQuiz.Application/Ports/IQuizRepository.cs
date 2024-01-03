using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Quizes;

namespace QuoteQuiz.Application.Ports
{
    public interface IQuizRepository
    {
        void AddQuiz(Quiz quiz);
        Task<Quiz?> GetAsync(int quizId);
        Task<IList<Quiz>> GetListAsync(QuizFilter filter);
    }
}
