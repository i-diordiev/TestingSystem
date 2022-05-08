using TestingSystem.DAL.Entities;
using TestingSystem.DAL.Interfaces;

namespace TestingSystem.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;

        private IRepository<Answer> _answerRepository;
        private IRepository<Question> _questionRepository;
        private IRepository<Test> _testRepository;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
        }

        public IRepository<Answer> Answers
        {
            get
            {
                if (_answerRepository == null)
                    _answerRepository = new AnswerRepository(_context);
                return _answerRepository;
            }
        }

        public IRepository<Question> Questions
        {
            get
            {
                if (_questionRepository == null)
                    _questionRepository = new QuestionRepository(_context);
                return _questionRepository;
            }
        }

        public IRepository<Test> Tests
        {
            get
            {
                if (_testRepository == null)
                    _testRepository = new TestRepository(_context);
                return _testRepository;
            }
        }
    }
}