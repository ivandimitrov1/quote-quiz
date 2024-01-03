using QuoteQuiz.Application.Domain;
using System.ComponentModel.DataAnnotations;

namespace QuoteQuiz.Api.QuizManagement.ViewModels
{
    public class EditQuizViewModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        public List<EditQuoteViewModel> Quotes { get; set; }

        public static EditQuizViewModel ToViewModel(Quiz quiz)
        {
            return new EditQuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                Quotes = quiz.Quotes?.Select(x => new EditQuoteViewModel
                {
                    Id = x.Id,
                    Text = x.Text,
                    Answers = x.Answers?.Select((value, index) =>
                    {
                        return new EditQuoteAnswerViewModel
                        {
                            Id = index,
                            Text = value,
                            IsCorrect = x.IsCorrect(index),
                        };
                    }).ToList(),
                }).ToList()
            };
        }
    }

    public class EditQuoteViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Quote text is required.")]
        public string Text { get; set; }

        [Required(ErrorMessage = "Answers field should have at least one item.")]
        public List<EditQuoteAnswerViewModel> Answers { get; set; }
    }

    public class EditQuoteAnswerViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
