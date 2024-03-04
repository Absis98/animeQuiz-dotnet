using System;
using System.Collections.Generic;
using System.Linq;
using MyWebAPI.Data;

namespace MyWebAPI.Services
{
    public class QuestionService
    {
        private readonly ApplicationDbContext _context;

        private readonly QuizTypeService _quizTypeService;

        public QuestionService(ApplicationDbContext context, QuizTypeService quizTypeService)
        {
            _context = context;
            _quizTypeService = quizTypeService;
        }

        public Question GetQuestion(int id)
        {
            return _context.Question.Find(id);
        }

        public List<QuestionWithoutAnswerModel> GetRandomQuestions(int count, int quizType)
        {
            var questions = _context.Question
                             .Where(q => q.QuizType == quizType)
                             .ToList();

            _quizTypeService.increaseAttemptCount(quizType);
            if (questions.Count < count)
            {
                // Handle the case where there are fewer questions available than requested
                // You can either adjust the count or handle it according to your requirements
                count = questions.Count;
            }
            Random rand = new Random();
            var randomIndices = Enumerable.Range(0, questions.Count)
                                        .OrderBy(x => rand.Next())
                                        .Take(count)
                                        .ToList();

            return randomIndices.Select(index => new QuestionWithoutAnswerModel
                                                {
                                                    Id = questions[index].Id,
                                                    QuestionText = questions[index].QuestionText,
                                                    ImageURL = questions[index].ImageURL,
                                                    Options = questions[index].Options,
                                                })
                                .ToList();
        }

        public void AddQuestion(Question question)
        {
            if (question != null)
            {
                _context.Question.Add(question);
                _context.SaveChanges();
            }
            else
            {
                throw new ArgumentNullException(nameof(question), "Invalid Question details");
            }
        }

        public List<VerificationResult> VerifyAnswers(List<AnswerWithQuestionId> data)
        {
            var verificationResults = new List<VerificationResult>();

            foreach (var qa in data)
            {
                // Retrieve the question from the database based on the provided ID
                var question = _context.Question.FirstOrDefault(q => q.Id == qa.Id);
                // If the question with the provided ID is not found, set verification result as false
                if (question == null)
                {
                    verificationResults.Add(new VerificationResult {id = qa.Id, isCorrect = false});
                }
                else
                {
                    // Compare the provided answer with the answer from the database
                    // Assuming case-insensitive comparison
                    verificationResults.Add(new VerificationResult {id = qa.Id, isCorrect = string.Equals(question.Answer, qa.Answer, StringComparison.OrdinalIgnoreCase)});
                }
            }

            return verificationResults;
        }
    }
}
