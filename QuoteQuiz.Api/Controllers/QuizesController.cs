using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuoteQuiz.Api.Quizes.ViewModels;
using QuoteQuiz.Api.QuizManagement.ViewModels;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.Quizes;
using QuoteQuiz.Application.Quizes.Interfaces;

namespace QuoteQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(RoleEnum.User) + "," + nameof(RoleEnum.Admin))]
    public class QuizesController : Controller
    {
        private readonly IQuizService _quizService;
        private readonly IQuizRepository _quizRepository;
        private readonly IUserRepository _userRepository;

        public QuizesController(
            IQuizService quizService,
            IQuizRepository quizRepository,
            IUserRepository userRepository)
        {
            _quizService = quizService;
            _quizRepository = quizRepository;
            _userRepository = userRepository;
        }

        [HttpGet("mylist")]
        public async Task<ActionResult> GetMyList()
        {
            var currentUser = await _userRepository.GetAsync(CurrentUserId());
            return Ok(currentUser.GetQuizesInProgress());
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetList()
        {
            QuizFilter filter = new QuizFilter() { Published = true };
            IList<Quiz> quizes = await _quizRepository.GetListAsync(filter);
            return Ok(QuizListItemViewModel.ToViewModel(quizes));
        }

        [HttpGet("{quizId}/nextquote")]
        public async Task<ActionResult> GetNextQuote(int quizId)
        {
            var nextQuote = await _quizService.GetNextQuoteAsync(CurrentUserId(), quizId);
            return Ok(GetNextQuoteViewModel.ToViewModel(nextQuote));
        }

        [HttpPost("{quizId}/start")]
        public async Task<ActionResult> StartQuiz(int quizId)
        {
            await _quizService.StartQuizAsync(CurrentUserId(), quizId);
            return await GetNextQuote(quizId);
        }

        [HttpPost("{quizId}/quote/{quoteId}/answer")]
        public async Task<ActionResult> Answer([FromBody] AnswerQuoteViewModel answerViewModel)
        {
            var answer = await _quizService.AnswerQuoteAsync(
                    CurrentUserId(),
                    answerViewModel.QuizId,
                    answerViewModel.QuoteId,
                    answerViewModel.UserAnswer);

            return Ok(AnswerQuoteResultViewModel.ToViewModel(
                            answer.QuizId,
                            answer.Quote,
                            answer.Answer.Value));
        }

        private int CurrentUserId()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == nameof(QuoteQuiz.Application.Domain.User.Id)).Value);
        }
    }
}
