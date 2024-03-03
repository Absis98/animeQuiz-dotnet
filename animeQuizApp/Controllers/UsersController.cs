using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Data;

namespace MyWebAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

// GET: api/Users/5
        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _context.User.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
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
