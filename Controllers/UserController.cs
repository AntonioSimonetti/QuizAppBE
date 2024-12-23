using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Interfaces;

namespace QuizApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userProfile = await _userService.GetUserProfileAsync(User);

            if (userProfile == null)
            {
                return NotFound("User not found.");
            }

            return Ok(userProfile);

        }

        [HttpGet("email-confirmation-status")]
        public async Task<IActionResult> GetEmailConfirmationStatus()
        {
            var isConfirmed = await _userService.GetEmailConfirmationStatusAsync(User);
            return Ok(new { isConfirmed = isConfirmed });
        }

    }
}
