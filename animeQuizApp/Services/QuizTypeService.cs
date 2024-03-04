using System;
using System.Collections.Generic;
using System.Linq;
using MyWebAPI.Data;
using System.Text.Json;

namespace MyWebAPI.Services
{
    public class QuizTypeService
    {
        private readonly ApplicationDbContext _context;

        public QuizTypeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<QuizType> getQuizTypes()
        {
            var quizTypes = _context.QuizType.ToList();
            return quizTypes;
        }

        public void increaseAttemptCount(int quizTypeId) {
            var quizType = _context.QuizType.FirstOrDefault(quizType => quizType.Id == quizTypeId);

            if (quizType != null) {
                quizType.TotalAttempts = quizType.TotalAttempts + 1;
                _context.SaveChanges();
            }
        }
        
    }
}
