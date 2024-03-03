using System;
using System.Collections.Generic;
using System.Linq;
using MyWebAPI.Data;
using System.Text.Json;

namespace MyWebAPI.Services
{
    public class QuizSessionService
    {
        private readonly ApplicationDbContext _context;

        public QuizSessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int setQuizSession(List<int> questionIds)
        {
            var quizSessionData = new QuizSession {
                CreateTime = DateTime.Now,
                Questions = string.Join(",", questionIds),
                QuizName = "Anime"
            };

            _context.QuizSession.Add(quizSessionData);
            _context.SaveChanges();
            return quizSessionData.Id;
        }

        public void submitAnswersAndScoreData(AnswerData data, int score) {
            var quizSessionData = _context.QuizSession.FirstOrDefault(e => e.Id == data.SessionId);

            if (quizSessionData != null)
            {
                // Update the entity
                quizSessionData.Answers = JsonSerializer.Serialize(data.Answers);
                quizSessionData.Score = score;

                // Save changes
                _context.SaveChanges();
            }
        }

        
    }
}
