using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuoteQuiz.Api.UserReview.ViewModels;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.Quizes;
using System.Data;

namespace QuoteQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserReviewController : Controller
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IUserRepository _userRepository;

        public UserReviewController(
            IQuizRepository quizRepository,
            IUserRepository userRepository)
        {
            _quizRepository = quizRepository;
            _userRepository = userRepository;
        }

        [HttpGet("list")]
        public async Task<ActionResult> Get()
        {
            var currentUser = await _userRepository.GetAsync(CurrentUserId());
            if (currentUser.Role == RoleEnum.Admin)
            {
                var allUsers = await _userRepository.GetUsersAsync();
                return await GetUserReviews(allUsers);
            }
            
            return await GetUserReviews(new List<User> { currentUser });
        }

        private async Task<ActionResult> GetUserReviews(IList<User> users)
        {
            var quizes = await _quizRepository.GetListAsync(new QuizFilter { Published = true });

            var userReviews = new List<UserQuizReviewViewModel>();
            foreach (var user in users)
            {
                foreach (var quiz in quizes)
                {
                    var userQuizReviews = user.GetQuizReviews(quiz);
                    userReviews.AddRange(
                        userQuizReviews.Select(
                            x =>
                            new UserQuizReviewViewModel
                            {
                                UserId = x.UserId,
                                Login = x.UserLogin,
                                QuizTitle = x.QuizTitle,
                                CorrectAnswers = x.CorrectAnswers,
                                QuoteCount = x.QuoteCount,
                                StartedOn = x.StartedOn,
                            }));
                }
            }

            return Ok(userReviews);
        }
        private int CurrentUserId()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == nameof(QuoteQuiz.Application.Domain.User.Id)).Value);
        }
    }
}
