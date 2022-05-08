using System;
using System.Linq;
using NUnit.Framework;
using TestingSystem.BLL.Interfaces;
using TestingSystem.BLL.Services;
using TestingSystem.DAL;
using TestingSystem.DAL.Repositories;

namespace TestingSystem.Tests
{
    public class QuestionServiceTests
    {
        public IQuestionService GetService()
        {
            var context = new ApplicationContext();
            var unit = new UnitOfWork(context);
            var service = new QuestionService(unit);
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
        public void QuestionService_FindQuestionById_Existing()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddQuestion("test");
            // Act
            var actual = service.FindQuestionById(expected.Id);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void QuestionService_FindQuestionById_NotExisting()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.FindQuestionById(-1);
            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void QuestionService_GetAllQuestions_NotEmpty()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddQuestion("test");
            // Act
            var actual = service.GetAllQuestions();
            // Assert
            Assert.Contains(expected, actual.ToList());
            Assert.IsNotNull(actual.FirstOrDefault().Answers);
        }

        [Test]
        public void QuestionService_GetAllQuestions_Empty()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.GetAllQuestions();
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);
        }

        [Test]
        public void QuestionService_AddQuestion()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.AddQuestion("test");
            // Assert
            Assert.IsNotNull(actual);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void QuestionService_AddAnswerToQuestion_NotExisting(int answerId, int questionId)
        {
            // Arrange
            var service = GetService();
            var expectedExType = typeof(Exception);
            // Act
            var actual = Assert.Catch(() => service.AddAnswerToQuestion(questionId, answerId));
            // Assert
            Assert.AreEqual(expectedExType, actual.GetType());
        }

        [Test]
        public void QuestionService_AddAnswerToQuestion_Existing()
        {
            // Arrange
            var context = new ApplicationContext();
            var unit = new UnitOfWork(context);
            var service = new QuestionService(unit);
            var answerService = new AnswerService(unit);
            AutoId.Reset();
            var question = service.AddQuestion("test");
            var answer = answerService.AddAnswer("test", 0, true);
            // Act
            var actual = service.AddAnswerToQuestion(question.Id, answer.Id);
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual.Answers);
            Assert.AreEqual(1, actual.Answers.FirstOrDefault().PositionInQuestion);
        }

        [TestCase(-1, 1)]
        [TestCase(1, -1)]
        public void QuestionService_RemoveAnswerFromQuestion_NotExisting(int answerId, int questionId)
        {
            // Arrange
            var service = GetService();
            var expectedExType = typeof(Exception);
            // Act
            var actual = Assert.Catch(() => service.RemoveAnswerFromQuestion(questionId, answerId));
            // Assert
            Assert.AreEqual(expectedExType, actual.GetType());
        }

        [Test]
        public void QuestionService_RemoveAnswerFromQuestion_Existing()
        {
            // Arrange
            var context = new ApplicationContext();
            var unit = new UnitOfWork(context);
            var service = new QuestionService(unit);
            var answerService = new AnswerService(unit);
            AutoId.Reset();
            var question = service.AddQuestion("test");
            var answer1 = answerService.AddAnswer("test1", 0, true);
            var answer2 = answerService.AddAnswer("test2", 0, false);
            service.AddAnswerToQuestion(question.Id, answer1.Id);
            service.AddAnswerToQuestion(question.Id, answer2.Id);
            // Act
            var actual = service.RemoveAnswerFromQuestion(question.Id, answer1.Id);
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual.Answers);
            Assert.AreEqual(1, actual.Answers.FirstOrDefault().PositionInQuestion);

        }

        [Test]
        public void QuestionService_UpdateQuestionText_NotExisting()
        {
            // Arrange
            var service = GetService();
            var expectedExType = typeof(Exception);
            // Act
            var actual = Assert.Catch(() => service.UpdateQuestionText(-1, "test"));
            // Assert
            Assert.AreEqual(expectedExType, actual.GetType());
        }

        [Test]
        public void QuestionService_UpdateQuestionText_Existing()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddQuestion("test");
            // Act
            expected.QuestionText = "test2";
            var actual = service.UpdateQuestionText(expected.Id, "test2");
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void QuestionService_DeleteQuestion_NotExisting()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.DeleteQuestion(-1);
            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void QuestionService_DeleteQuestion_Existing()
        {
            // Arrange
            var service = GetService();
            var question = service.AddQuestion("test");
            // Act
            var actual = service.DeleteQuestion(question.Id);
            // Assert
            Assert.IsTrue(actual);
        }
    }
}