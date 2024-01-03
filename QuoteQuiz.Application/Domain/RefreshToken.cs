using QuoteQuiz.Api.Core;
using System.Net;

namespace QuoteQuiz.Application.Domain
{
    public class RefreshToken
    {
        public RefreshToken() { }

        public RefreshToken(string token)
        {
            TokenHash = SecurityUtils.GetHash(token);

            Expires = DateTime.UtcNow.AddDays(7);
            Created = DateTime.UtcNow;
        }

        public int Id { get; private set; }
        public string TokenHash { get; private set; }
        public DateTime Expires { get; private set; }
        public DateTime Created { get; private set; }

        public void SetAsExpired()
        {
            // set date in the past
            Expires = DateTime.UtcNow.AddDays(-8);
        }
    }
}
