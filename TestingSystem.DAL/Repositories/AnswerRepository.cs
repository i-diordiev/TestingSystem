using System;
using System.Collections.Generic;
using System.Linq;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.DAL.Repositories
{
    public class AnswerRepository : IRepository<Answer>
    {
        private readonly ApplicationContext _context;

        public AnswerRepository(ApplicationContext context)
        {
            _context = context;
        }
        public Answer Find(int id)
        {
            var answers = _context.Answers;
            return answers.FirstOrDefault(a => a.Id == id);
        }

        public bool Add(Answer item)
        {
            var answers = _context.Answers;
            answers.Add(item);
            if (answers.Contains(item))
                return true;
            return false;
        }

        public Answer Update(Answer item)
        {
            var answers = _context.Answers;
            var answerToUpdate = answers.FirstOrDefault(a => a.Id == item.Id);

            if (answerToUpdate == null)
                throw new Exception("Answer not found.");

            answerToUpdate.QuestionId = item.QuestionId;
            answerToUpdate.PositionInQuestion = item.PositionInQuestion;
            answerToUpdate.AnswerText = item.AnswerText;
            answerToUpdate.IsCorrect = item.IsCorrect;

            return answerToUpdate;
        }

        public bool Delete(int id)
        {
            var answers = _context.Answers;
            var answerToDelete = answers.FirstOrDefault(a => a.Id == id);

            if (answerToDelete == null)
                return false;

            answers.Remove(answerToDelete);
            return true;
        }

        public IEnumerable<Answer> GetAll()
        {
            var answers = _context.Answers;
            return answers.ToList();
        }
    }
}