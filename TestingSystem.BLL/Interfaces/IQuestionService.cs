using System.Collections.Generic;
using TestingSystem.DAL.Entities;

namespace TestingSystem.BLL.Interfaces
{
    public interface IQuestionService
    {
        Question FindQuestionById(int id);
        IEnumerable<Question> GetAllQuestions();
        Question AddQuestion(string text);
        Question AddAnswerToQuestion(int questionId, int answerId);
        Question RemoveAnswerFromQuestion(int questionId, int answerId);
        Question UpdateQuestionText(int questionId, string text);
        bool DeleteQuestion(int id);
    }
}