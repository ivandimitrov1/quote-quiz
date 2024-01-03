using QuoteQuiz.Application.Domain;

namespace QuoteQuiz.Api.QuizManagement.ViewModels
{
    public class QuizListItemViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Published { get; set; }
        public int QuoteCount { get; set; }
        public bool Started { get; set; }
        public static IList<QuizListItemViewModel> ToViewModel(IList<Quiz> list)
        {
            return list.Select(ToViewModel).ToList();
        }

        public static QuizListItemViewModel ToViewModel(Quiz quiz)
        {
            return new QuizListItemViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Published = quiz.Published,
                QuoteCount = quiz.Quotes.Count,
            };
        }
    }
}
