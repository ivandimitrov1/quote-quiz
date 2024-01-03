using QuoteQuiz.Api.Core;

namespace QuoteQuiz.Application.Service.Security
{
    public static class CookieUtils
    {
        public static void AppendExpiredTokenCookies(HttpResponse response, string token)
        {
            CookieOptions cookieTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(-10),
            };

            response.Cookies.Append(SecurityUtils.RefreshTokenCookieName, token, cookieTokenOptions);

            CookieOptions cookieTokenExistOptions = new CookieOptions
            {
                HttpOnly = false,
                Expires = DateTime.UtcNow.AddDays(-10),
            };

            response.Cookies.Append(SecurityUtils.RefreshTokenCookieExist, "false", cookieTokenExistOptions);
        }

        public static void AppendTokenCookies(HttpResponse response, string token)
        {
            DateTime expires = DateTime.UtcNow.AddDays(7);
            CookieOptions cookieTokenOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
            };

            response.Cookies.Append(SecurityUtils.RefreshTokenCookieName, token, cookieTokenOptions);

            CookieOptions cookieTokenExistOptions = new CookieOptions
            {
                HttpOnly = false,
                Expires = expires,
            };

            response.Cookies.Append(SecurityUtils.RefreshTokenCookieExist, "true", cookieTokenExistOptions);
        }
    }
}
