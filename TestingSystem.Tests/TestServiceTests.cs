using System;
using System.Linq;
using NUnit.Framework;
using TestingSystem.BLL.Interfaces;
using TestingSystem.BLL.Services;
using TestingSystem.DAL;
using TestingSystem.DAL.Repositories;

namespace TestingSystem.Tests
{
    public class TestServiceTests
    {
        public ITestService GetService()
        {
            var context = new ApplicationContext();
            var unit = new UnitOfWork(context);
            var service = new TestService(unit);
            AutoId.Reset();
            return service;
        }
        
        [Test]
        public void SimpleTest()
        {
            // Arrange

            // Act

            // Assert
            Assert.True(true);
        }

        [Test]
        public void TestService_FindTestById_Existing()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddTest("test", 2);
            // Act
            var actual = service.FindTestById(expected.Id);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestService_FindTestById_NotExisting()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.FindTestById(-1);
            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void TestService_Search_WrongQuery()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.SearchForTestsByTitle("114124134 fsdfsdf");
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);
        }

        [Test]
        public void TestService_Search_CorrectQuery()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddTest("test", 2);
            // Act
            var actual = service.SearchForTestsByTitle("test");
            // Assert
            Assert.Contains(expected, actual.ToList());
        }

        [Test]
        public void TestService_GetAllTests_Empty()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.GetAllTests();
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);
        }

        [Test]
        public void TestService_GetAllTests_NotEmpty()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddTest("test", 2);
            // Act
            var actual = service.GetAllTests();
            // Assert
            Assert.Contains(expected, actual.ToList());
            Assert.IsNotNull(actual.FirstOrDefault().Questions);
        }

        [Test]
        public void TestService_AddTest()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.AddTest("test", 2);
            // Assert
            Assert.IsNotNull(actual);
        }
                 

        [TestCase(1, -1)]
        [TestCase(-1, 1)]
        public void TestService_AddQuestionToTest_NotExisting(int questionId, int testId)
        {
            // Arrange
            var service = GetService();
            var expectedExType = typeof(Exception);
            // Act
            var actual = Assert.Catch(() => service.AddQuestionToTest(testId, questionId));
            // Assert
            Assert.AreEqual(expectedExType, actual.GetType());
        }

        [Test]
        public void TestService_AddQuestionToTest_Existing()
        {
            // Arrange
            var context = new ApplicationContext();
            var unit = new UnitOfWork(context);
            var service = new TestService(unit);
            var questionService = new QuestionService(unit);
            AutoId.Reset();
            var question = questionService.AddQuestion("test");
            var test = service.AddTest("test", 2);
            // Act
            var actual = service.AddQuestionToTest(test.Id, question.Id);
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual.Questions);
            Assert.AreEqual(1, actual.Questions.FirstOrDefault().PositionInTest);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void TestService_RemoveQuestionFromTest_NotExisting(int questionId, int testId)
        {
            // Arrange
            var service = GetService();
            var expectedExType = typeof(Exception);
            // Act
            var actual = Assert.Catch(() => service.RemoveQuestionFromTest(testId, questionId));
            // Assert
            Assert.AreEqual(expectedExType, actual.GetType());
        }

        [Test]
        public void TestService_RemoveQuestionFromTest_Existing()
        {
            // Arrange
            var context = new ApplicationContext();
            var unit = new UnitOfWork(context);
            var service = new TestService(unit);
            var questionService = new QuestionService(unit);
            AutoId.Reset();
            var question1 = questionService.AddQuestion("test1");
            var question2 = questionService.AddQuestion("test2");
            var test = service.AddTest("test", 2);
            service.AddQuestionToTest(test.Id, question1.Id);
            service.AddQuestionToTest(test.Id, question2.Id);
            // Act
            var actual = service.RemoveQuestionFromTest(test.Id, question1.Id);
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual.Questions);
            Assert.AreEqual(1, actual.Questions.FirstOrDefault().PositionInTest);
        }

        [Test]
        public void TestService_Update_NotExisting()
        {
            // Arrange
            var service = GetService();
            var expectedExType = typeof(Exception);
            // Act
            var actual = Assert.Catch(() => service.UpdateTestTime(-1, 2));
            // Assert
            Assert.AreEqual(expectedExType, actual.GetType());
        }

        [Test]
        public void TestService_Update_Title()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddTest("test", 2);
            // Act
            expected.Title = "test3";
            var actual = service.UpdateTestTitle(expected.Id, "test3");
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestService_Update_Time()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddTest("test", 2);
            // Act
            expected.TimeForOneQuestion = 3;
            var actual = service.UpdateTestTime(expected.Id, 3);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void TestService_Delete_NotExisting()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.DeleteTest(-1);
            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void TestService_Delete_Existing()
        {
            // Arrange
            var service = GetService();
            var test = service.AddTest("test", 1);
            // Act
            var actual = service.DeleteTest(test.Id);
            // Assert
            Assert.IsTrue(actual);
        }
    }
}