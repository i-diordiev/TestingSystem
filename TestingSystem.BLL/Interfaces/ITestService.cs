using System.Collections.Generic;
using TestingSystem.DAL.Entities;

namespace TestingSystem.BLL.Interfaces
{
    public interface ITestService
    {
        Test FindTestById(int id);
        IEnumerable<Test> SearchForTestsByTitle(string title);
        IEnumerable<Test> GetAllTests();
        Test AddTest(string title, int timeForOneQuestion);
        Test AddQuestionToTest(int testId, int questionId);
        Test RemoveQuestionFromTest(int testId, int questionId);
        Test UpdateTestTitle(int id, string title);
        Test UpdateTestTime(int id, int time);
        bool DeleteTest(int id);
    }
}