namespace QuoteQuiz.Application.Domain
{
    public class UserAnswer
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public int? QuoteId { get; set; }
        public Quote? Quote { get; set; }
        public AnswerEnum? Answer { get; set; }
        public DateTime OnDate { get; set; }
        public bool? QuizFinished { get; set; }
    }
}
