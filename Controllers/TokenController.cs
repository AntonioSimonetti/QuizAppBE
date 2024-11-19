using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("ValidateToken")]
        [Authorize] // Protezione automatica con Identity per ora usiamo questo approccio per validare o meno il token
        public IActionResult ValidateToken()
        {
            return Ok(true); // Se il token è valido, ritorna true
        }

    }
}
