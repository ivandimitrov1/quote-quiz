namespace QuoteQuiz.Application.Domain
{
    public class UserQuizReview
    {
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string QuizTitle { get; set; }
        public int CorrectAnswers { get; set; }
        public int QuoteCount { get; set; }
        public DateTime StartedOn { get; set; }
    }
}
