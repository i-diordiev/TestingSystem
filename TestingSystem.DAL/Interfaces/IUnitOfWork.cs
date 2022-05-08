using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Answer> Answers { get; }
        IRepository<Question> Questions { get; }
        IRepository<Test> Tests { get; }
    }
}