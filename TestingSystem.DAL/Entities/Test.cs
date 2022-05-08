using System.Collections.Generic;

namespace TestingSystem.DAL.Entities
{
    public class Test : BaseEntity
    {
        public string Title { get; set; }
        public int TimeForOneQuestion { get; set; }
        public ICollection<Question> Questions { get; set; }

        public Test()
        {
            Id = AutoId.TestId;
        }
    }
}