using System.Collections.Generic;
using TestingSystem.DAL.Entities;

namespace TestingSystem.DAL
{
    public class ApplicationContext
    {
        public List<Answer> Answers { get; set; }
        public List<Question> Questions { get; set; }
        public List<Test> Tests { get; set; }

        public ApplicationContext()
        {
            Answers = new List<Answer>();
            Questions = new List<Question>();
            Tests = new List<Test>();
        }
    }
}