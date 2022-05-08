using System;
using System.Collections.Generic;
using System.Linq;
using TestingSystem.BLL.Interfaces;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.BLL.Services
{
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unit;

        public TestService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public Test FindTestById(int id)
        {
            return _unit.Tests.Find(id);
        }

        public IEnumerable<Test> SearchForTestsByTitle(string title)
        {
            var tests = _unit.Tests.GetAll().Where(t => t.Title.Contains(title));
            return tests;
        }

        public IEnumerable<Test> GetAllTests()
        {
            return _unit.Tests.GetAll();
        }

        public Test AddTest(string title, int timeForOneQuestion)
        {
            Test test = new Test()
            {
                Title = title,
                TimeForOneQuestion = timeForOneQuestion
            };

            var result = _unit.Tests.Add(test);
            if (result)
                return test;
            else
                throw new Exception("Test adding failed.");
        }

        public Test AddQuestionToTest(int testId, int questionId)
        {
            var test = _unit.Tests.Find(testId);
            var question = _unit.Questions.Find(questionId);

            if (test == null)
                throw new Exception("Test not found.");
            if (question == null)
                throw new Exception("Question not found.");
            if (test.Questions.FirstOrDefault(q => q.Id == questionId) != null)
                throw new Exception("This question is already presented in this test.");

            question.TestId = test.Id;
            question.PositionInTest = test.Questions.Count + 1;

            _unit.Questions.Update(question);

            return _unit.Tests.Find(testId);
        }

        public Test RemoveQuestionFromTest(int testId, int questionId)
        {
            var test = _unit.Tests.Find(testId);
            var question = _unit.Questions.Find(questionId);

            if (test == null)
                throw new Exception("Test not found.");
            if (question == null)
                throw new Exception("Question not found.");

            question.TestId = 0;
            question.PositionInTest = 0;

            test = _unit.Tests.Find(testId);
            int pos = 1;
            foreach (var q in test.Questions)
            {
                q.PositionInTest = pos;
                pos++;
                _unit.Questions.Update(q);
            }
            
            return _unit.Tests.Find(testId);
        }

        public Test UpdateTestTitle(int id, string title)
        {
            var test = _unit.Tests.Find(id);
            
            if (test == null)
                throw new Exception("Test not found.");

            test.Title = title;
            return _unit.Tests.Update(test);
        }

        public Test UpdateTestTime(int id, int time)
        {
            var test = _unit.Tests.Find(id);
            
            if (test == null)
                throw new Exception("Test not found.");

            test.TimeForOneQuestion = time;
            return _unit.Tests.Update(test);
        }

        public bool DeleteTest(int id)
        {
            return _unit.Tests.Delete(id);
        }
    }
}