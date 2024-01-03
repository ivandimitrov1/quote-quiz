namespace QuoteQuiz.Application.Service.Security
{
    public class Password
    {
        private const int WorkFactor = 12;

        public Password(string password, string salt = null)
        {
            Salt = string.IsNullOrEmpty(salt) ?
                BCrypt.Net.BCrypt.GenerateSalt(WorkFactor) : salt;

            // Hash the password with the salt
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, Salt);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Verify the password against the stored hash
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string PasswordHash { get; }
        public string Salt { get; }
    }
}
