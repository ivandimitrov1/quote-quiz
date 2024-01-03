using QuoteQuiz.Application.Common;
using System.Linq;

namespace QuoteQuiz.Application.Domain
{
    public class Quiz
    {
        /// <summary>
        /// ef core usage
        /// </summary>
        public Quiz() { }

        /// <summary>
        /// mapping usage
        /// </summary>
        public Quiz(string title, List<Quote>? quotes = null, int id = 0) 
        {
            Id = id;
            SetTitle(title);

            Quotes = new List<Quote>();
            SetQuotes(quotes);
            Published = false;
        }

        public int Id { get; set; }
        public string Title { get; private set; }
        public bool Published { get; private set; }
        public List<Quote> Quotes { get; private set; }

        public void Publish()
        {
            if (Quotes.Count == 0)
            {
                throw new QuoteQuizApplicationException("Quiz without quotes cant be published");
            }

            Published = true;
        }

        public Quote? GetQuote(int quoteId)
        {
            return Quotes.FirstOrDefault(x => x.Id == quoteId);
        }

        public bool IsLast(int? quoteId)
        {
            return GetQuoteAfter(quoteId) == null;
        }

        public Quote? GetQuoteAfter(int? quoteId)
        {
            if (quoteId == null)
            {
                return Quotes.OrderBy(x => x.Id).FirstOrDefault();
            }

            return Quotes.OrderBy(x => x.Id).FirstOrDefault(x => x.Id > quoteId);
        }

        public void Unpublish()
        {
            Published = false;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new QuoteQuizApplicationException("Title should not be empty.");
            }

            Title = title;
        }

        public void SetQuotes(List<Quote> changedQuotes)
        {
            if (changedQuotes == null)
            {
                return;
            }

            foreach (Quote quote in changedQuotes)
            {
                var existingQuote = Quotes.FirstOrDefault(x => quote.Id != 0 && x.Id == quote.Id);
                if (existingQuote == null)
                {
                    quote.QuizId = Id;
                    Quotes.Add(quote);
                }
                else
                {
                    existingQuote.Text = quote.Text;
                    existingQuote.CorectAnswer = quote.CorectAnswer;
                    existingQuote.Answers = quote.Answers;
                }
            }

            for (int i = Quotes.Count - 1; i >= 0; i--)
            {
                Quote itemToBeRemoved = Quotes[i];
                if (changedQuotes.All(p => p.Id != itemToBeRemoved.Id))
                {
                    Quotes.Remove(itemToBeRemoved);
                }
            }
        }
    }
}
