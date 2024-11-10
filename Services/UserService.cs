using Microsoft.AspNetCore.Identity;
using QuizApp.Interfaces;
using QuizApp.Data;
using System.Threading.Tasks;
using QuizApp.Models;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data.DTO;

namespace QuizApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }

            return new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            };
        }
    }
}
