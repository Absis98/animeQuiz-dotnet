using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Data;

namespace MyWebAPI.Controllers
{
    [Route("api/validate-user")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult ValidateUser([FromBody] Login loginData)
        {
            if (loginData == null)
            {
                return BadRequest("Invalid user details");
            }

            var existingUser = _context.User.FirstOrDefault(u => u.Email == loginData.Email && u.Password == loginData.Password);

            if (existingUser == null)
            {
                return NotFound("User not found");
            }

            return Ok(existingUser);
        }

    }
}
