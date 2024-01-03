using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Quizes;
using QuoteQuiz.Application.Service.Security;

namespace QuoteQuiz.Application.Domain
{
    public class User
    {
        /// <summary>
        /// EF core usage
        /// </summary>
        public User() { }

        public User(
            string login,
            string password,
            string name,
            RoleEnum? role = null)
        {
            if (string.IsNullOrEmpty(login))
            {
                throw new ArgumentNullException("Login cant be empty.");
            }

            Login = login;
            Name = name;

            var userPassword = new Password(password);
            Salt = userPassword.Salt;
            PasswordHash = userPassword.PasswordHash;
            Role = role ?? RoleEnum.User;
            RefreshTokens = new List<RefreshToken>();
            Answers = new List<UserAnswer>();
        }

        public int Id { get; private set; }
        public string Login { get; private set; }
        public string Name { get; set; }
        public string PasswordHash { get; private set; }
        public string Salt { get; private set; }
        public RoleEnum Role { get; set; }
        public bool Deleted { get; private set; }
        public List<RefreshToken> RefreshTokens { get; private set; }
        public List<UserAnswer> Answers { get; private set; }

        private UserAnswer GetQuizStart(int quizId)
        {
            return Answers.FirstOrDefault(x => x.QuizId == quizId && x.QuoteId == null && x.QuizFinished == false);
        }

        public List<UserAnswer> GetQuiz(int quizId)
        {
            return Answers.Where(x => x.QuizId == quizId && x.QuoteId != null).ToList();
        }

        public List<UserQuizReview> GetQuizReviews(Quiz quiz)
        {
            if (!Answers.Any(x => x.QuizId == quiz.Id && x.QuizFinished == true))
            {
                return new List<UserQuizReview>();
            }

            // to:do 
            // change the structure so that we can group them somehow
            var userQuizAnswers = Answers.Where(x => x.QuizId == quiz.Id).OrderBy(x => x.Id);

            List<UserQuizReview> reviews = new List<UserQuizReview>();
            UserQuizReview currentReview = null;
            foreach (var answer in userQuizAnswers)
            {
                if (answer.QuizFinished == true)
                {
                    currentReview = new UserQuizReview();
                    currentReview.UserId = Id;
                    currentReview.QuizTitle = quiz.Title;
                    currentReview.UserLogin = Login;
                    currentReview.StartedOn = answer.OnDate;
                    reviews.Add(currentReview);
                    continue;
                } 
                else if (answer.QuoteId != null)
                {
                    var quote = quiz.GetQuote(answer.QuoteId.Value);
                    if (quote.CorectAnswer == answer.Answer.Value)
                    {
                        currentReview.CorrectAnswers++;
                    }

                    currentReview.QuoteCount++;
                }

                if (answer.QuizFinished == false)
                {
                    break;
                }
            }

            return reviews;
        }

        public List<int> QuizIn()
        {
            return Answers
                .GroupBy(x => x.QuizId)
                .Select(x => x.Key)
                .ToList();
        }

        public List<int> GetQuizesInProgress()
        {
            return Answers
                .Where(x => x.QuizFinished == false)
                .GroupBy(x => x.QuizId)
                .Select(x => x.Key)
                .ToList();
        }

        public bool HasQuizInProgress(int quizId)
        {
            return GetQuizStart(quizId) != null;
        }

        public bool QuoteAlreadyGiven(int quizId, int quoteId)
        {
            return GetAnsweredQuote(quizId, quoteId) != null;
        }

        public AnsweredQuote GetAnsweredQuote(int quizId, int quoteId)
        {
            var answer = Answers
                .Where(x => x.QuizId == quizId && x.QuoteId == quoteId)
                .OrderByDescending(x => x.OnDate)
                .FirstOrDefault();

            if (answer == null)
            {
                throw new QuoteQuizApplicationException("The quote hasnt been answered.");
            }

            return new AnsweredQuote { QuoteId = quoteId, UserAnswer = answer.Answer };
        }

        public void FinishQuiz(int quizId)
        {
            if (HasQuizInProgress(quizId))
            {
                var quizStart = GetQuizStart(quizId);
                quizStart.QuizFinished = true;
                return;
            }

            throw new QuoteQuizApplicationException("Quiz should be started first.");
        }

        public UserAnswer StartQuiz(int quizId)
        {
            if (Answers.Any(x => x.QuizId == quizId && x.QuizFinished == false))
            {
                throw new QuoteQuizApplicationException("Quiz has already been started.");
            }

            var userAnswer = new UserAnswer();
            userAnswer.QuizId = quizId;
            userAnswer.OnDate = DateTime.UtcNow;
            userAnswer.QuizFinished = false;
            Answers.Add(userAnswer);
            return userAnswer;
        }

        public UserAnswer Answer(
            int quizId,
            int quoteId,
            AnswerEnum answer)
        {
            if (!HasQuizInProgress(quizId))
            {
                throw new QuoteQuizApplicationException($"User hasnt taken the quiz {quizId}.");
            }

            var userAnswer = new UserAnswer();
            userAnswer.QuizId = quizId;
            userAnswer.QuoteId = quoteId;
            userAnswer.Answer = answer;
            userAnswer.OnDate = DateTime.UtcNow;
            Answers.Add(userAnswer);
            return userAnswer;
        }

        public int? GetLastAnsweredQuoteId(int quizId)
        {
            return Answers.OrderByDescending(x => x.Id).FirstOrDefault()?.QuoteId;
        }

        public void Delete(int executedByUserId)
        {
            if (executedByUserId == Id)
            {
                throw new QuoteQuizApplicationException("User cannot delete itself.");
            }


            Deleted = true;
            // TO:DO
            // refresh token are not hard deleted 
            // but only user id is removed, find better solution
            RefreshTokens.Clear();
        }

        public bool IsRefreshTokenExpired(string tokenHash)
        {
            var refreshToken = RefreshTokens.FirstOrDefault(x => x.TokenHash == tokenHash);
            return refreshToken == null || DateTime.UtcNow >= refreshToken.Expires;
        }

        public void SetRefreshTokenAsExpired(string tokenHash)
        {
            var refreshToken = RefreshTokens.FirstOrDefault(x => x.TokenHash == tokenHash);
            refreshToken?.SetAsExpired();
        }

        public void SetRole(RoleEnum role, int executedByUserId)
        {
            if (executedByUserId == Id && 
                role != Role)
            {
                throw new QuoteQuizApplicationException("Current user cannot change its privileges.");
            }
            
            Role = role;
        }

        public void AddRefreshToken(RefreshToken refreshToken)
        {
            RefreshTokens.Add(refreshToken);
        }
    }
}
