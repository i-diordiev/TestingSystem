namespace TestingSystem.DAL
{
    public static class AutoId
    {
        private static int _answerId = 1;
        private static int _questionId = 1;
        private static int _testId = 1;

        public static int AnswerId => _answerId++;
        public static int QuestionId => _questionId++;
        public static int TestId => _testId++;

        public static void Reset()
        {
            _answerId = 1;
            _questionId = 1;
            _testId = 1;
        }
    }
}