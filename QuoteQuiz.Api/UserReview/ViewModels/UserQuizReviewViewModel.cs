namespace QuoteQuiz.Api.UserReview.ViewModels
{
    public class UserQuizReviewViewModel
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public string QuizTitle { get; set; }
        public int CorrectAnswers { get; set; }
        public int QuoteCount { get; set; }
        public DateTime StartedOn { get; set; }
    }
}
