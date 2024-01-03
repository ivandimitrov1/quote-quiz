namespace QuoteQuiz.Application.Domain
{
    public class NextQuote
    {
        public Quiz Quiz { get; set; }
        public Quote? Quote { get; set; }
        public bool IsLast { get; set; }
    }
}
