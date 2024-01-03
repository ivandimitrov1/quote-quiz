using Microsoft.AspNetCore.Mvc;
using QuoteQuiz.Api.Core;
using Microsoft.AspNetCore.Authorization;
using QuoteQuiz.Api.UserManagement.ViewModels;
using QuoteQuiz.Application.Common;
using QuoteQuiz.Application.Domain;
using System.Data;
using QuoteQuiz.Application.Ports;
using QuoteQuiz.Application.UserManagement.Interfaces;
using QuoteQuiz.Application.UserManagement;

namespace QuoteQuiz.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = nameof(RoleEnum.Admin))]
    public class UserManagementController : Controller
    {
        private readonly IUserManagementService _userManagementService;
        private readonly IUserRepository _userRepository;

        public UserManagementController(
            IUserManagementService userManagementService,
            IUserRepository getUserDataService)
        {
            _userManagementService = userManagementService;
            _userRepository = getUserDataService;
        }

        [HttpGet("list")]
        public async Task<ActionResult> GetList(
            int? pageId = 0,
            int? pageSize = 10,
            [FromQuery] OrderBy? orderBy = null,
            [FromQuery] UserFilter? filter = null)
        {
            PaginationSettings paging = new PaginationSettings(pageId, pageSize);
            var users = await _userRepository.GetUsersAsync(filter, orderBy, paging);

            return Ok(users.Select(UserDataViewModel.ToViewModel).ToList());
        }

        [HttpPut("{userId}")]
        public async Task<ActionResult> Update(UpdateUserViewModel userDataViewModel)
        {
            var user = await _userRepository.GetAsync(userDataViewModel.Id);
            if (user == null)
            {
                return NotFound();
            }

            var savedUser = await _userManagementService.SaveUserAsync(
                    userDataViewModel.Id,
                    userDataViewModel.Name,
                    (RoleEnum)userDataViewModel.Role.Id,
                    CurrentUserId());
            return Ok(UserDataViewModel.ToViewModel(savedUser));
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> Delete(int userId)
        {
            await _userManagementService.DeleteUserAsync(userId, CurrentUserId());
            return Ok($"User with {userId} was deleted.");
        }

        [HttpPut("{userId}/disable")]
        public async Task<ActionResult> Disable(int userId)
        {
            await _userManagementService.DisableUserAsync(userId, CurrentUserId());
            return Ok($"User with {userId} was deleted.");
        }

        private int CurrentUserId()
        {
            return Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == nameof(QuoteQuiz.Application.Domain.User.Id)).Value);
        }
    }
}
