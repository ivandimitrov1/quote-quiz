using QuoteQuiz.Application.Common;
using System;
using System.Reflection.Metadata.Ecma335;

namespace QuoteQuiz.Application.Domain
{
    public class Quote
    {
        /// <summary>
        /// ef core usage
        /// </summary>
        public Quote() { }

        /// <summary>
        ///  mapping usage
        /// </summary>
        public Quote(
            int id,
            string text,
            List<QuoteAnswer> answers)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new QuoteQuizApplicationException("Quote text cant be empty.");
            }

            Id = id;
            Text = text;
            SetAnswers(answers);
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public AnswerEnum CorectAnswer { get; set; }
        public List<string> Answers { get; set; }
        public int QuizId { get; set; }

        public bool SameAs(int? quoteId)
        {
            return Id == quoteId;
        }

        public bool IsCorrect(int index)
        {
            if (IsYesOrNoQuote())
            {
                return Convert.ToBoolean((int)CorectAnswer);
            }

             return (index + 1) == (int)CorectAnswer;
        }

        public string GetCorrectAnswer()
        {
            if (IsYesOrNoQuote())
            {
                return CorectAnswer == AnswerEnum.No ? "No" : "Yes";
            }

            return Answers[(int)CorectAnswer - 1];
        }

        public bool IsYesOrNoQuote()
        {
            return Answers.Count == 1;
        }

        public bool IsCorrect(AnswerEnum? userAnswer)
        {
            if (userAnswer == null)
            {
                return false;
            }

            return CorectAnswer == userAnswer;
        }

        private void SetAnswers(List<QuoteAnswer> answers)
        {
            if (answers == null || answers.Count == 0)
            {
                throw new QuoteQuizApplicationException("Quote must have answers.");
            }

            bool isYesNoQuote = answers.Count == 1;
            if (isYesNoQuote)
            {
                var onlyOneOption = answers.FirstOrDefault();
                CorectAnswer = onlyOneOption.IsCorrect ? AnswerEnum.First : AnswerEnum.No;
                Answers = answers.Select(x => x.Text).ToList();
                return;
            }

            bool multipleChoiceQuote = answers.Count > 1;
            bool moreThanOneCorrectAnswer = answers.Where(x => x.IsCorrect).Count() > 1;
            bool noAnswers = answers.Where(x => x.IsCorrect).Count() == 0;
            if (multipleChoiceQuote && (moreThanOneCorrectAnswer || noAnswers))
            {
                throw new QuoteQuizApplicationException("Quote can have only one correct answer.");
            }

            var answerIndex = answers.Single(x => x.IsCorrect).Index + 1;
            CorectAnswer = (AnswerEnum)answerIndex;

            Answers = answers.Select(x => x.Text).ToList();
        }
    }
}
