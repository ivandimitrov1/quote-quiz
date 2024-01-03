using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuoteQuiz.Api.QuizManagement.ViewModels;
using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.QuizManagement.Interfaces;

namespace QuoteQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(RoleEnum.Admin))]
    public class QuizManagementController : Controller
    {
        private readonly IQuizManagementService _quizManagementService;
        private readonly IQuizRepository _quizRepository;
        
        public QuizManagementController(
            IQuizManagementService quizManagementService,
            IQuizRepository quizRepository)
        {
            _quizManagementService = quizManagementService;
            _quizRepository = quizRepository;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetList()
        {
            IList<Quiz> quizes = await _quizRepository.GetListAsync(null);
            return Ok(QuizListItemViewModel.ToViewModel(quizes));
        }

        [HttpGet("{quizId}")]
        public async Task<ActionResult> Get(int quizId)
        {
            if (quizId <= 0)
            {
                return ValidationProblem("Quiz id should be filled.");
            }

            Quiz? quiz = await _quizRepository.GetAsync(quizId);
            if (quiz == null) 
            {
                return NotFound();
            }

            return Ok(EditQuizViewModel.ToViewModel(quiz));
        }

        [HttpPost]
        public async Task<ActionResult> Create(EditQuizViewModel editQuiz)
        {
            if (editQuiz.Id != 0)
            {
                throw new QuoteQuizApplicationException("Cant create new quiz with existing ids.");
            }

            return await Save(editQuiz);
        }

        [HttpPut]
        public async Task<ActionResult> Save(EditQuizViewModel editQuiz)
        {
            List<Quote>? quotes = editQuiz
                                    .Quotes?
                                    .Select(x => new Quote(
                                        x.Id,
                                        x.Text,
                                        x.Answers.Select(
                                            y => new QuoteAnswer(y.Id, y.Text, y.IsCorrect)).ToList()))
                                    .ToList();
            Quiz quiz = new Quiz(
                            editQuiz.Title,
                            quotes,
                            editQuiz.Id);

            await _quizManagementService.SaveAsync(quiz);
            return Ok(EditQuizViewModel.ToViewModel(quiz));
        }

        [HttpPut("{quizId}/publish")]
        public async Task<ActionResult> Publish(int quizId)
        {
            bool published = await _quizManagementService.PublishAsync(quizId);
            return Ok("Published.");
        }

        [HttpPut("{quizId}/unpublish")]
        public async Task<ActionResult> UnpublishAsync(int quizId)
        {
            bool published = await _quizManagementService.UnpublishAsync(quizId);
            return Ok("Unpublished.");
        }
    }
}
