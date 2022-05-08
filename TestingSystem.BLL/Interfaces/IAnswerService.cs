using System.Collections.Generic;
using TestingSystem.DAL.Entities;

namespace TestingSystem.BLL.Interfaces
{
    public interface IAnswerService
    {
        Answer FindAnswerById(int id);
        IEnumerable<Answer> GetAllAnswers();
        Answer AddAnswer(string text, int position, bool isCorrect);
        Answer UpdateAnswerText(int id, string text);
        Answer UpdateAnswerCorrectness(int id, bool isCorrect);
        bool DeleteAnswer(int id);
    }
}