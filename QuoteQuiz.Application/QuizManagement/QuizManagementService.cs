using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.QuizManagement.Interfaces;

namespace QuoteQuiz.Application.QuizManagement
{
    public class QuizManagementService : IQuizManagementService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IAsyncDbContext _asyncDbContext;

        public QuizManagementService(
            IQuizRepository quizRepository,
            IAsyncDbContext asyncDbContext)
        {
            _quizRepository = quizRepository;
            _asyncDbContext = asyncDbContext;
        }

        public async Task<bool> UnpublishAsync(int quizId)
        {
            var quiz = await _quizRepository.GetAsync(quizId);
            if (quiz == null)
            {
                return false;
            }

            quiz.Unpublish();
            await _asyncDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PublishAsync(int quizId)
        {
            var quiz = await _quizRepository.GetAsync(quizId);
            if (quiz == null)
            {
                return false;
            }

            quiz.Publish();
            await _asyncDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Quiz?> SaveAsync(Quiz quiz)
        {
            if (quiz == null)
            {
                throw new ArgumentNullException("Quiz should not be null.");
            }

            if (quiz.Published == true)
            {
                throw new QuoteQuizApplicationException("Quiz cant be changed while it is published."); ;
            }

            var storedQuiz = await _quizRepository.GetAsync(quiz.Id);
            if (storedQuiz == null)
            {
                _quizRepository.AddQuiz(quiz);
                await _asyncDbContext.SaveChangesAsync();
                return quiz;
            }

            storedQuiz.SetTitle(quiz.Title);
            storedQuiz.SetQuotes(quiz.Quotes);
            await _asyncDbContext.SaveChangesAsync();

            return storedQuiz;
        }
    }
}
