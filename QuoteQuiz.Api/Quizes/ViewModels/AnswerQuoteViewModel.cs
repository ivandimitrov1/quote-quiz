using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Api.Quizes.ViewModels
{
    public class AnswerQuoteViewModel
    {
        public int QuizId { get; set; }
        public int QuoteId { get; set; }
        public AnswerEnum UserAnswer { get; set; }
    }

    public class AnswerQuoteResultViewModel : AnswerQuoteViewModel
    {
        public bool? IsCorrect { get; set; }
        public string CorrectAnswer { get; set; }

        public static AnswerQuoteResultViewModel ToViewModel(
            int quizId,
            Quote quote,
            AnswerEnum userAnswer)
        {
            return new AnswerQuoteResultViewModel
            {
                QuizId = quizId,
                QuoteId = quote.Id,
                UserAnswer = userAnswer,
                IsCorrect = quote.IsCorrect(userAnswer),
                CorrectAnswer = quote.GetCorrectAnswer()
            };
        }
    }
}
