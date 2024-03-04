using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Data;
using MyWebAPI.Services;

namespace MyWebAPI.Controllers
{
    [Route("api/quiz-types")]
    [ApiController]
    public class QuizTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly QuizTypeService _quizTypeService;
        public QuizTypeController(ApplicationDbContext context, QuizTypeService quizTypeService)
        {
            _context = context;
            _quizTypeService= quizTypeService;
        }

        [HttpGet("list")]
        public IActionResult GetQuizTypes()
        {
            var types = _quizTypeService.getQuizTypes();
            if (types == null)
            {
                return StatusCode(500, "No data found in quiz types db");
            }

            return Ok(types);
        }
    }

}
