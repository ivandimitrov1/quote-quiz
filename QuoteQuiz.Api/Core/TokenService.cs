using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuoteQuiz.Api.Core
{
    public interface ITokenService
    {
        string GenerateAccessToken(List<Claim> claims);
    }

    public class TokenService : ITokenService
    {
        private readonly IOptions<JwtOptions> _jwtOptions;

        public TokenService(
            IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }

        public string GenerateAccessToken(List<Claim> claims)
        {
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Value.SecurityKey);
            var secret = new SymmetricSecurityKey(key);
            var signingCredentials = new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(2),
                    signingCredentials: signingCredentials);

            string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return accessToken;
        }
    }
}
