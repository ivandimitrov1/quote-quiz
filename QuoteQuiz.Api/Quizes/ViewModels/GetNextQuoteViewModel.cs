using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Api.Quizes.ViewModels
{
    public class GetNextQuoteViewModel
    {
        public int QuizId { get; set; }
        public string QuizTitle { get; set; }
        public int QuoteId { get; set; }
        public string QuoteText { get; set; }
        public List<string> QuoteAnswers { get; set; }
        public bool IsLast { get; set; }
        public bool IsYesNoQuote { get; set; }

        public static GetNextQuoteViewModel ToViewModel(NextQuote nextQuote)
        {
            return new GetNextQuoteViewModel
            {
                QuizId = nextQuote.Quiz.Id,
                QuizTitle = nextQuote.Quiz.Title,

                QuoteId = nextQuote.Quote.Id,
                QuoteText = nextQuote.Quote?.Text,
                QuoteAnswers = nextQuote.Quote.Answers.Select(x => x).ToList(),
                IsLast = nextQuote.IsLast,
                IsYesNoQuote = nextQuote.Quote.IsYesOrNoQuote()
            };
        }
    }
}
