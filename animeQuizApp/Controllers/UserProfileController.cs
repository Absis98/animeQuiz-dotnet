using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Data;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
    [Route("api/user-profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly QuizSessionService _quizSessionService;
        public UserProfileController(ApplicationDbContext context, QuizSessionService quizSessionService)
        {
            _context = context;
            _quizSessionService = quizSessionService;
        }

        [HttpGet("sessions/{id}")]
        public IActionResult GetSessionsForUser(int id)
        {
            var quizSessions = _quizSessionService.getSessionsByUserId(id);

            if (quizSessions == null)
            {
                return NotFound();
            }

            return Ok(quizSessions);
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult AddUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user details");
            }

            _context.User.Add(user);
            _context.SaveChanges();

            return Ok("User added successfully");
        }

    }
}
