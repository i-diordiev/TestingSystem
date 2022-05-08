using System.Collections.Generic;

namespace TestingSystem.DAL.Entities
{
    public class Question : BaseEntity
    {
        public int TestId { get; set; }
        public int PositionInTest { get; set; }
        public string QuestionText { get; set; }
        public ICollection<Answer> Answers { get; set; }

        public Question()
        {
            Id = AutoId.QuestionId;
            TestId = 0;
            PositionInTest = 0;
        }
    }
}