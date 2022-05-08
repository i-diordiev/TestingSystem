using System;
using System.Collections.Generic;
using TestingSystem.BLL.Interfaces;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.BLL.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unit;

        public AnswerService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public Answer FindAnswerById(int id)
        {
            return _unit.Answers.Find(id);
        }

        public IEnumerable<Answer> GetAllAnswers()
        {
            return _unit.Answers.GetAll();
        }

        public Answer AddAnswer(string text, int position, bool isCorrect)
        {
            Answer answer = new Answer()
            {
                AnswerText = text,
                IsCorrect = isCorrect,
                PositionInQuestion = position
            };

            var result = _unit.Answers.Add(answer);
            if (result)
                return answer;
            else
                throw new Exception("Answer adding failed.");
        }

        public Answer UpdateAnswerText(int id, string text)
        {
            var answer = _unit.Answers.Find(id);
            if (answer == null)
                throw new Exception("Answer not found.");

            answer.AnswerText = text;
            return _unit.Answers.Update(answer);
        }

        public Answer UpdateAnswerCorrectness(int id, bool isCorrect)
        {
            var answer = _unit.Answers.Find(id);
            if (answer == null)
                throw new Exception("Answer not found.");

            answer.IsCorrect = isCorrect;
            return _unit.Answers.Update(answer);
        }

        public bool DeleteAnswer(int id)
        {
            return _unit.Answers.Delete(id);
        }
    }
}