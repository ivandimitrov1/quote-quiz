using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.Quizes.Interfaces;

namespace QuoteQuiz.Application.Quizes
{
    public class QuizService : IQuizService
    {
        private readonly IUserRepository _userRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IAsyncDbContext _asyncDbContext;
        public QuizService(
            IUserRepository userRepository,
            IAsyncDbContext asyncDbContext,
            IQuizRepository quizRepository)
        {
            _userRepository = userRepository;
            _asyncDbContext = asyncDbContext;
            _quizRepository = quizRepository;
        }

        public async Task<NextQuote> GetNextQuoteAsync(int userId, int quizId)
        {
            var user = await _userRepository.GetAsync(userId);
            var lastAnsweredQuoteId = user.GetLastAnsweredQuoteId(quizId);

            if (!user.HasQuizInProgress(quizId))
            {
                throw new QuoteQuizApplicationException("Quiz is not started yet.");
            }

            var quiz = await _quizRepository.GetAsync(quizId);
            NextQuote nextQuote = new NextQuote();
            nextQuote.Quiz = quiz;
            nextQuote.Quote = quiz.GetQuoteAfter(lastAnsweredQuoteId);
            nextQuote.IsLast = nextQuote.Quote == null || quiz.IsLast(nextQuote.Quote.Id);

            return nextQuote;
        }

        public async Task<AnsweredQuote> GetAnsweredQuoteAsync(int userId, int quizId, int quoteId)
        {
            var user = await _userRepository.GetAsync(userId);

            if (!user.HasQuizInProgress(quizId))
            {
                throw new QuoteQuizApplicationException($"User hasnt taken the quiz {quizId}");
            }

            var answeredQuote = user.GetAnsweredQuote(quizId, quoteId);
            return new AnsweredQuote { QuoteId = answeredQuote.QuoteId, UserAnswer = answeredQuote.UserAnswer };
        }

        public async Task<bool> StartQuizAsync(int userId, int quizId)
        {
            var quiz = await _quizRepository.GetAsync(quizId);
            if (!quiz.Published)
            {
                throw new QuoteQuizApplicationException("Quiz is not published yet.");
            }

            var user = await _userRepository.GetAsync(userId);
            user.StartQuiz(quizId);
            await _asyncDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserAnswer> AnswerQuoteAsync(
            int userId,
            int quizId,
            int quoteId,
            AnswerEnum userAnswer)
        {
            var user = await _userRepository.GetAsync(userId);
            var nextQuote = await GetNextQuoteAsync(userId, quizId);

            if ((nextQuote.Quote != null && nextQuote.Quote.SameAs(quoteId)) 
                || user.QuoteAlreadyGiven(quizId, quoteId))
            {
                var answer = user.Answer(quizId, quoteId, userAnswer);
             
                if (nextQuote.IsLast)
                {
                    user.FinishQuiz(quizId);
                }

                await _asyncDbContext.SaveChangesAsync();
                return answer;
            }

            throw new QuoteQuizApplicationException($"User cant answer to this quote yet.");
        }
    }

}
