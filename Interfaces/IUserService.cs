using QuizApp.Data.DTO;
using System.Security.Claims;

namespace QuizApp.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserProfileAsync(ClaimsPrincipal user);
    }
}
