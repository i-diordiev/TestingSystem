using System;
using System.Linq;
using NUnit.Framework;
using TestingSystem.BLL.Interfaces;
using TestingSystem.BLL.Services;
using TestingSystem.DAL;
using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Repositories;

namespace TestingSystem.Tests
{
    public class AnswerServiceTests
    {
        public IAnswerService GetService()
        {
            var context = new ApplicationContext();
            var unit = new UnitOfWork(context);
            var service = new AnswerService(unit);
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
        public void AnswerService_FindAnswerById_Existing()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddAnswer("test", 1, true);
            // Act
            var actual = service.FindAnswerById(expected.Id);
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AnswerService_FindAnswerById_NotExisting()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.FindAnswerById(-1);
            // Assert
            Assert.IsNull(actual);
        }

        [Test] 
        public void AnswerService_GetAllAnswers_Empty()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.GetAllAnswers();
            // Assert
            Assert.IsNotNull(actual);
            Assert.IsEmpty(actual);
        }

        [Test]
        public void AnswerService_GetAllAnswers_NotEmpty()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddAnswer("test", 1, true);
            // Act
            var actual = service.GetAllAnswers();
            // Assert
            Assert.Contains(expected, actual.ToList());
        }

        [Test]
        public void AnswerService_AddAnswer_Normal()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.AddAnswer("test", 1, true);
            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void AnswerService_Update_NotExisting()
        {
            // Arrange
            var service = GetService();
            var expectedExType = typeof(Exception);
            // Act
            Answer answer = new Answer();
            var actual = Assert.Catch(() => service.UpdateAnswerText(answer.Id, "tetet"));
            // Assert
            Assert.AreEqual(expectedExType, actual.GetType());
        }

        [Test]
        public void AnswerService_Update_Text()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddAnswer("test", 1, true);
            // Act
            expected.AnswerText = "test2";
            var actual = service.UpdateAnswerText(expected.Id, "test2");
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AnswerService_Update_Correctness()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddAnswer("test", 1, true);
            // Act
            expected.IsCorrect = false;
            var actual = service.UpdateAnswerCorrectness(expected.Id, false);
            // Assert
            Assert.AreEqual(expected, actual);
        }
        

        [Test]
        public void AnswerService_DeleteAnswer_NotExisting()
        {
            // Arrange
            var service = GetService();
            // Act
            var actual = service.DeleteAnswer(-1);
            // Assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void AnswerService_DeleteAnswer_Existing()
        {
            // Arrange
            var service = GetService();
            var expected = service.AddAnswer("test", 1, true);
            // Act
            var actual = service.DeleteAnswer(expected.Id);
            // Assert
            Assert.IsTrue(actual);
        }
    }
}