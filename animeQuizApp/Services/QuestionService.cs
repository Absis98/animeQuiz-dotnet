using System;
using System.Collections.Generic;
using System.Linq;
using MyWebAPI.Data;

namespace MyWebAPI.Services
{
    public class QuestionService
    {
        private readonly ApplicationDbContext _context;

        public QuestionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Question GetQuestion(int id)
        {
            return _context.Question.Find(id);
        }

        public List<QuestionWithoutAnswerModel> GetRandomQuestions(int count)
        {
            int totalRows = _context.Question.Count();
            Random rand = new Random();
            var randomIndices = Enumerable.Range(0, totalRows)
                                          .OrderBy(x => rand.Next())
                                          .Take(count)
                                          .ToList();

            return _context.Question
                            .OrderBy(q => q.Id)
                            .Where(q => randomIndices.Contains(q.Id))
                            .Select(entity => new QuestionWithoutAnswerModel
                            {
                                Id = entity.Id,
                                QuestionText = entity.QuestionText,
                                ImageURL = entity.ImageURL,
                                Options = entity.Options,
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
