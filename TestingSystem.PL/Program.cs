using TestingSystem.BLL.Services;
using TestingSystem.DAL;
using TestingSystem.DAL.Repositories;

namespace TestingSystem.PL
{
    class Program
    {
        static void Main(string[] args)
        {
            var context = new ApplicationContext();
            var unit = new UnitOfWork(context);
            var answerService = new AnswerService(unit);
            var questionService = new QuestionService(unit);
            var testService = new TestService(unit);
            var menu = new Menu(answerService, questionService, testService);
            menu.Show();
        }
    }
}