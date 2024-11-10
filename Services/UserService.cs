using Microsoft.AspNetCore.Identity;
using QuizApp.Interfaces;
using QuizApp.Data;
using System.Threading.Tasks;
using QuizApp.Models;
using Microsoft.EntityFrameworkCore;
using QuizApp.Data.DTO;
using System.Security.Claims;


namespace QuizApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(ClaimsPrincipal user)
        {
            // Estrai il userId dal ClaimsPrincipal (può essere 'Id' o 'unique_name' a seconda della configurazione dei claim)
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return null; // Se non c'è un userId nei claims, restituisci null
            }

            // Recupera l'utente dal database utilizzando il userId
            var applicationUser = await _userManager.FindByIdAsync(userId);

            if (applicationUser == null)
            {
                return null; // Se l'utente non esiste nel database, restituisci null
            }

            // Crea un oggetto DTO con i dati dell'utente
            return new UserProfileDto
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email
            };
        }
    }
}
