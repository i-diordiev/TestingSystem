using System;
using System.Collections.Generic;
using System.Linq;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.DAL.Repositories
{
    public class QuestionRepository : IRepository<Question>
    {
        private readonly ApplicationContext _context;

        public QuestionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Question Find(int id)
        {
            var questions = _context.Questions;
            var question = questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
                question.Answers = _context.Answers.Where(a => a.QuestionId == question.Id).ToList();
            return question;
        }

        public bool Add(Question item)
        {
            var questions = _context.Questions;
            questions.Add(item);
            if (questions.Contains(item))
                return true;
            return false;
        }

        public Question Update(Question item)
        {
            var questions = _context.Questions;
            var questionToUpdate = questions.FirstOrDefault(q => q.Id == item.Id);

            if (questionToUpdate == null)
                throw new Exception("Question not found.");

            questionToUpdate.TestId = item.TestId;
            questionToUpdate.PositionInTest = item.PositionInTest;
            questionToUpdate.Answers = item.Answers;
            questionToUpdate.QuestionText = item.QuestionText;
            
            return questionToUpdate;
        }

        public bool Delete(int id)
        {
            var questions = _context.Questions;
            var questionToDelete = questions.FirstOrDefault(q => q.Id == id);

            if (questionToDelete == null)
                return false;

            questions.Remove(questionToDelete);
            return true;
        }

        public IEnumerable<Question> GetAll()
        {
            var questions = _context.Questions.ToList();
            questions.ForEach(q => q.Answers = _context.Answers.Where(a => a.QuestionId == q.Id).ToList());
            return questions;
        }
    }
}