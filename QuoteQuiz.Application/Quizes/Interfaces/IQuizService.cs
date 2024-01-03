using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Application.Quizes.Interfaces
{
    public interface IQuizService
    {
        Task<bool> StartQuizAsync(int userId, int quizId);
        Task<NextQuote?> GetNextQuoteAsync(int userId, int quizId);
        Task<AnsweredQuote> GetAnsweredQuoteAsync(int userId, int quizId, int quoteId);
        Task<UserAnswer> AnswerQuoteAsync(
            int userId,
            int quizId,
            int quoteId,
            AnswerEnum answer);
    }
}
