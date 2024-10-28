using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace QuizApp.Controllers
{
    /*
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ConfigTestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigTestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("test-environment")]
        public IActionResult TestEnvironmentVariables()
        {
            var email = _configuration["EmailSettings:Email"];
            var password = _configuration["EmailSettings:Password"]; // dovrei passarla nascosta quantomento e non in chiaro sulla rete
           

            return Ok(new
            {
                Email = email,
                Password = password,
            });
        }
    }
    */
}

