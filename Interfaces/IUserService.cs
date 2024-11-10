using QuizApp.Data.DTO;

namespace QuizApp.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserProfileAsync(string userId);
    }
}
