namespace QuoteQuiz.Api.Core
{
    public class JwtOptions
    {
        public static string SectionKey { get; } = "Auth:Jwt";

        public string SecurityKey { get; set; }
    }
}
