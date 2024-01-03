using QuoteQuiz.Application.Common;

namespace QuoteQuiz.Application.Domain
{
    public class QuoteAnswer
    {
        public QuoteAnswer(int index, string text, bool isCorrect)
        {
            if (string.IsNullOrEmpty(text)) 
            {
                throw new QuoteQuizApplicationException("Quote answer cant be empty.");
            }

            Index = index;
            Text = text;
            IsCorrect = isCorrect;
        }

        public int Index { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
