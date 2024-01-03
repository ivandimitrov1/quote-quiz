using System.Security.Cryptography;
using System.Text;

namespace QuoteQuiz.Api.Core
{
    public static class SecurityUtils
    {
        public const string RefreshTokenCookieName = "quote-quiz-refresh-token";
        public const string RefreshTokenCookieExist = "quote-quiz-refresh-token-exist";

        public static string GetHash(string text)
        {
            return Convert.ToBase64String(
                new SHA512Managed().ComputeHash(Encoding.Unicode.GetBytes(text)));
        }
    }
}
