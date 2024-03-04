using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Data;
using MyWebAPI.Services;
using System.Linq;
using System.Reflection;

namespace MyWebAPI.Controllers
{
    [Route("api/question")]
    [ApiController]
    public class QuestionDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly QuestionService _questionService;
        private readonly QuizSessionService _quizSessionService;

        public QuestionDetailsController(ApplicationDbContext context, QuestionService questionService, QuizSessionService quizSessionService)
        {
            _context = context;
            _questionService = questionService;
            _quizSessionService = quizSessionService;
        }

        [HttpGet("{id}")]
        public IActionResult GetQuestion(int id)
        {
            var question = _questionService.GetQuestion(id);

            if (question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        [HttpGet("{userId}/questions-list/{quizTypeId}")]
        public IActionResult GetQuestionsList(int userId, int quizTypeId)
        {
            var randomQuestions = _questionService.GetRandomQuestions(3, quizTypeId);
            var questionIds = randomQuestions.Select(c => c.Id).ToList();
            int sessionId = _quizSessionService.setQuizSession(questionIds, userId, quizTypeId);
            if (randomQuestions.Count == 0)
            {
                return NotFound();
            }

            var questionData = new QuestionsList {
                sessionId = sessionId,
                questions = randomQuestions
            };

            return Ok(questionData);
        }

        [HttpPost("verify-answers")]
        public IActionResult VerifyAnswers([FromBody] AnswerData data)
        {
            var questionAnswers = _questionService.VerifyAnswers(data.Answers);
            int score = 0;
            
            foreach (var qa in questionAnswers)
            {
                if (qa.isCorrect) {
                    score = score + 1;
                }
            }

            _quizSessionService.submitAnswersAndScoreData(data, score);
            return Ok(questionAnswers);
        }

        [HttpPost]
        public IActionResult AddQuestion([FromBody] Question question)
        {
           try
            {
                _questionService.AddQuestion(question);
                return Ok("Question added successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
