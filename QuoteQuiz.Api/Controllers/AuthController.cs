using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuoteQuiz.Api.Core;
using QuoteQuiz.Api.UserManagement.ViewModels;
using QuoteQuiz.Application.Domain;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.Service.Security;
using QuoteQuiz.Application.UserManagement.Interfaces;
using System.Security.Claims;

namespace QuoteQuiz.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IAsyncDbContext _asyncDbContext;
        public AuthController(
            IUserManagementService userManagementService,
            IUserRepository userRepository,
            ITokenService tokenService,
            IAsyncDbContext asyncDbContext)
        {
            _userManagementService = userManagementService;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _asyncDbContext = asyncDbContext;
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            string token = Request.Cookies[SecurityUtils.RefreshTokenCookieName];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Refresh token cookie is missing." });
            }

            string tokenHash = SecurityUtils.GetHash(token);
            User? user = await _userRepository.GetUserByRefreshTokenAsync(tokenHash);

            if (user == null)
            {
                return NotFound("User is not found.");
            }

            if (user.IsRefreshTokenExpired(tokenHash))
            {
                return Unauthorized("Refresh Token is not active.");
            }

            user.SetRefreshTokenAsExpired(tokenHash);
            await _asyncDbContext.SaveChangesAsync();

            CookieUtils.AppendExpiredTokenCookies(Response, token);

            return Ok(new { message = "Token revoked" });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginRestViewModel restUser)
        {
            User? user = await _userRepository.GetUserByLoginAsync(restUser.Login);

            if (user != null
                && user.Deleted == false
                && user.PasswordHash == new Password(restUser.Password, user.Salt).PasswordHash)
            {
                string accessToken = GenerateUserAccessToken(user);

                // TO:DO
                // refresh token is created on every login
                // see if we can reuse the alive refresh token
                var refreshTokenGuid = Guid.NewGuid().ToString("n");
                RefreshToken refreshToken = new RefreshToken(refreshTokenGuid);
                user.AddRefreshToken(refreshToken);
                await _asyncDbContext.SaveChangesAsync();
                CookieUtils.AppendTokenCookies(Response, refreshTokenGuid);

                return Ok(new AuthRestDto { Token = accessToken });
            }

            return ValidationProblem("Wrong username or password.");
        }

        [AllowAnonymous]
        [HttpPost("refreshtoken")]
        public async Task<ActionResult> RefreshToken()
        {
            string token = Request.Cookies[SecurityUtils.RefreshTokenCookieName];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            string tokenHash = SecurityUtils.GetHash(token);
            User? user = await _userRepository.GetUserByRefreshTokenAsync(tokenHash);

            if (user == null || user.Deleted 
                || user.IsRefreshTokenExpired(tokenHash))
            {
                CookieUtils.AppendExpiredTokenCookies(Response, token);
                return Ok();
            }

            string accessToken = GenerateUserAccessToken(user);
            return Ok(new AuthRestDto { Token = accessToken });
        }

        [HttpPost("create")]
        public async Task<ActionResult> Create(CreateUserViewModel restUser)
        {
            User user = await _userManagementService.SaveUserAsync(
                            restUser.Login,
                            restUser.Name,
                            restUser.Password);

            if (user == null)
            {
                return ValidationProblem("Login already exists.");
            }

            return Ok(UserDataViewModel.ToViewModel(user));
        }

        private string GenerateUserAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(nameof(Application.Domain.User.Id), user.Id.ToString()),
                new Claim(nameof(Application.Domain.User.Login), user.Login),
                new Claim(nameof(Application.Domain.User.Role), user.Role.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            return _tokenService.GenerateAccessToken(claims);
        }
    }
}
