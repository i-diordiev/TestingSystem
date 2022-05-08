using System;
using System.Collections.Generic;
using System.Linq;
using TestingSystem.BLL.Interfaces;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unit;

        public QuestionService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public Question FindQuestionById(int id)
        {
            return _unit.Questions.Find(id);
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return _unit.Questions.GetAll();
        }

        public Question AddQuestion(string text)
        {
            Question question = new Question() { QuestionText = text };

            var result = _unit.Questions.Add(question);
            if (result)
                return question;
            else
                throw new Exception("Answer adding failed.");
        }

        public Question AddAnswerToQuestion(int questionId, int answerId)
        {
            var question = _unit.Questions.Find(questionId);
            var answer = _unit.Answers.Find(answerId);

            if (question == null)
                throw new Exception("Question not found.");
            if (answer == null)
                throw new Exception("Answer not found.");
            if (question.Answers.FirstOrDefault(a => a.Id == answerId) != null)
                throw new Exception("This answer is already presented in this test.");

            answer.QuestionId = question.Id;
            answer.PositionInQuestion = question.Answers.Count + 1;

            _unit.Answers.Update(answer);
            
            return _unit.Questions.Find(questionId);
        }

        public Question RemoveAnswerFromQuestion(int questionId, int answerId)
        {
            var question = _unit.Questions.Find(questionId);
            var answer = _unit.Answers.Find(answerId);

            if (question == null)
                throw new Exception("Question not found.");
            if (answer == null)
                throw new Exception("Answer not found.");
            
            answer.QuestionId = 0;
            answer.PositionInQuestion = 0;

            _unit.Answers.Update(answer);

            int pos = 1;
            question = _unit.Questions.Find(questionId);
            foreach (var ans in question.Answers)
            {
                ans.PositionInQuestion = pos;
                pos++;
                _unit.Answers.Update(ans);
            }
            
            return _unit.Questions.Find(questionId);
        }

        public Question UpdateQuestionText(int questionId, string text)
        {
            var question = _unit.Questions.Find(questionId);

            if (question == null)
                throw new Exception("Question not found.");

            question.QuestionText = text;
            return _unit.Questions.Update(question);
        }

        public bool DeleteQuestion(int id)
        {
            return _unit.Questions.Delete(id);
        }
    }
}