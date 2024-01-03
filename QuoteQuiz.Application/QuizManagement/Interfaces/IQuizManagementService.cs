using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Application.QuizManagement.Interfaces
{
    public interface IQuizManagementService
    {
        Task<Quiz?> SaveAsync(Quiz quiz);

        Task<bool> PublishAsync(int quizId);

        Task<bool> UnpublishAsync(int quizId);
    }
}
