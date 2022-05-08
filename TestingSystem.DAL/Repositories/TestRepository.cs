using System;
using System.Collections.Generic;
using System.Linq;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.DAL.Repositories
{
    public class TestRepository : IRepository<Test>
    {
        private readonly ApplicationContext _context;

        public TestRepository(ApplicationContext context)
        {
            _context = context;
        }
        public Test Find(int id)
        {
            var tests = _context.Tests;
            var test = tests.FirstOrDefault(t => t.Id == id);
            if (test != null)
            {
                var questions = _context.Questions.Where(q => q.TestId == test.Id).ToList();
                questions.ForEach(q => q.Answers = _context.Answers.Where(a => a.QuestionId == q.Id).ToList());
                test.Questions = questions;
            }
            
            return test;
        }

        public bool Add(Test item)
        {
            var tests = _context.Tests;
            tests.Add(item);
            if (tests.Contains(item))
                return true;
            return false;
        }

        public Test Update(Test item)
        {
            var tests = _context.Tests;
            var testToUpdate = tests.FirstOrDefault(t => t.Id == item.Id);

            if (testToUpdate == null)
                throw new Exception("Test not found.");

            testToUpdate.Questions = item.Questions;
            testToUpdate.Title = item.Title;
            testToUpdate.TimeForOneQuestion = item.TimeForOneQuestion;

            return testToUpdate;
        }

        public bool Delete(int id)
        {
            var tests = _context.Tests;
            var testToDelete = tests.FirstOrDefault(t => t.Id == id);

            if (testToDelete == null)
                return false;

            tests.Remove(testToDelete);
            return true;
        }

        public IEnumerable<Test> GetAll()
        {
            var tests = _context.Tests.ToList();
            tests.ForEach(t =>
            {
                var questions = _context.Questions.Where(q => q.TestId == t.Id).ToList();
                questions.ForEach(q => q.Answers = _context.Answers.Where(a => a.QuestionId == q.Id).ToList());
                t.Questions = questions;
            });
            return tests;
        }
    }
}