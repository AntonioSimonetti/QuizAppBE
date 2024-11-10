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
            var userId = User.FindFirst("sub")?.Value;
            if(string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var userProfile = await _userService.GetUserProfileAsync(userId);
            if(userProfile == null)
            {
                return NotFound("User not found.");
            }

            return Ok(userProfile);


        }
    }
}
