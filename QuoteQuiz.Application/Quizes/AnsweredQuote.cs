using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Application.Quizes
{
    public class AnsweredQuote
    {
        public int QuoteId { get; set; }
        public AnswerEnum? UserAnswer { get; set; }
    }
}
