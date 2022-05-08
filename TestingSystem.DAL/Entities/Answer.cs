namespace TestingSystem.DAL.Entities
{
    public class Answer : BaseEntity
    {
        public int QuestionId { get; set; }
        public int PositionInQuestion { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }

        public Answer()
        {
            Id = AutoId.AnswerId;
            QuestionId = 0;
            PositionInQuestion = 0;
        }
    }
}