using System;
using System.Collections.Generic;
using System.Linq;
using TestingSystem.BLL.Interfaces;

namespace TestingSystem.PL
{
    public class Menu
    {
        private readonly IAnswerService _answerService;
        private readonly IQuestionService _questionService;
        private readonly ITestService _testService;
        private List<PassedTest> _passedTests;

        public Menu(IAnswerService answerService, 
            IQuestionService questionService, 
            ITestService testService)
        {
            _answerService = answerService;
            _questionService = questionService;
            _testService = testService;
            _passedTests = new List<PassedTest>();
        }

        private int GetIntFromUserInput()
        {
            {
                while (true)
                {
                    try
                    {
                        return Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Not valid input. Try again.");
                    }
                }
            }
        }

        private void HandleException(Exception exception)
        {
            ConsoleColor color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(exception.Message);
            Console.ForegroundColor = color;
        }

        
        public void Show()
        {
            bool isAlive = true;
            while (isAlive)
            {
                try
                {
                    Console.WriteLine();
                    Console.WriteLine("List of possible options: ");
                    Console.WriteLine("1 Pass the test");
                    Console.WriteLine("2 Manage tests");
                    Console.WriteLine("3 Manage questions");
                    Console.WriteLine("4 Manage answers");
                    Console.WriteLine("5 Show statistics");
                    Console.WriteLine("6 Exit");
                    Console.Write("Select option: ");
                    switch (GetIntFromUserInput())
                    {
                        case 1:
                            PassTest();
                            break;
                        case 2:
                            ShowTestsAdminMenu();
                            break;
                        case 3:
                            ShowQuestionsAdminMenu();
                            break;
                        case 4:
                            ShowAnswersAdminMenu();
                            break;
                        case 5:
                            ShowStatistics();
                            break;
                        case 6:
                            isAlive = false;
                            break;
                    }

                    Console.WriteLine("\n\n");
                }
                catch (Exception e)
                {
                    HandleException(e);
                }
            }
            
        }

        private void PassTest()
        {
            try
            {
                Console.WriteLine("List of available tests: ");
                var tests = _testService.GetAllTests();
                foreach (var t in tests)
                {
                    Console.WriteLine($"ID: {t.Id}    Title: {t.Title}    " +
                                      $"Time for each question: {t.TimeForOneQuestion}");
                }

                Console.Write("Enter ID of the tests: ");
                var testId = GetIntFromUserInput();
                var test = _testService.FindTestById(testId);

                if (test == null)
                {
                    Console.WriteLine("Wrong ID!");
                    return;
                }
                if (!test.Questions.Any())
                {
                    Console.WriteLine("Empty test!");
                    return;
                }
                
                var timeStart = DateTime.Now;
                int answersCount = 0;
                int correntAnswersCount = 0;

                Console.WriteLine();
                Console.WriteLine("Test started!");
                Console.WriteLine("You can always stop the test, just type \"0\"!!!");
                Console.WriteLine();
                foreach (var question in test.Questions)
                {
                    Console.WriteLine(question.QuestionText);
                    foreach (var questionAnswer in question.Answers.OrderBy(a => a.PositionInQuestion))
                        Console.WriteLine($"{questionAnswer.PositionInQuestion} {questionAnswer.AnswerText}");
                    Console.Write("Select answer number: ");
                    int ansNum = GetIntFromUserInput();
                    if (ansNum == 0)
                        return;
                    
                    var ans = question.Answers.FirstOrDefault(a => a.PositionInQuestion == ansNum);
                    if (ans != null && ans.IsCorrect)
                    {
                        Console.WriteLine("Correct!");
                        answersCount++;
                        correntAnswersCount++;
                    }
                    else
                    {
                        Console.WriteLine("Incorrect!");
                        answersCount++;
                    }
                }

                var timeEnd = DateTime.Now;
                var totalTime = timeEnd - timeStart;

                PassedTest passedTest = new PassedTest()
                {
                    Percentage = (int) (correntAnswersCount / answersCount) * 100,
                    TestTitle = test.Title,
                    TotalTime = totalTime
                };
                _passedTests.Add(passedTest);

                Console.WriteLine("Test passed! Your results: ");
                Console.WriteLine($"Test title: {passedTest.TestTitle}");
                Console.WriteLine($"Total time: {passedTest.TotalTime}");
                Console.WriteLine($"Percentage: {passedTest.Percentage}");
            }
            catch (Exception e)
            {
                HandleException(e);
            }
        }

        private void ShowTestsAdminMenu()
        {
            Console.WriteLine();
            Console.WriteLine("List of possible options: ");
            Console.WriteLine("1 View all tests");
            Console.WriteLine("2 Search for tests by title");
            Console.WriteLine("3 Add test");
            Console.WriteLine("4 Remove test");
            Console.WriteLine("5 Add question to test");
            Console.WriteLine("6 Remove question from test");
            Console.WriteLine("7 Change test title");
            Console.WriteLine("8 Change time for 1 question");
            Console.WriteLine("9 Exit");
            Console.Write("Select option: ");
            switch (GetIntFromUserInput())
            {
                case 1:
                {
                    Console.WriteLine("List of available tests: ");
                    var tests = _testService.GetAllTests();
                    foreach (var t in tests)
                    {
                        Console.WriteLine($"ID: {t.Id}    Title: {t.Title}    " +
                                          $"Time for each question: {t.TimeForOneQuestion}");
                    }
                    break;
                }
                case 2:
                {
                    Console.Write("Enter your request: ");
                    string request = Console.ReadLine();
                    var tests = _testService.SearchForTestsByTitle(request);
                    Console.WriteLine("Results: ");
                    foreach (var t in tests)
                    {
                        Console.WriteLine($"ID: {t.Id}    Title: {t.Title}    " +
                                          $"Time for each question: {t.TimeForOneQuestion}");
                    }
                    break;
                }
                case 3:
                {
                    Console.Write("Enter title: ");
                    string title = Console.ReadLine();

                    Console.Write("Enter time for each question: ");
                    int time = GetIntFromUserInput();

                    var test = _testService.AddTest(title, time);
                    Console.WriteLine($"ID: {test.Id}    Title: {test.Title}    " +
                                      $"Time for each question: {test.TimeForOneQuestion}");
                    break;
                }
                    
                case 4:
                {
                    Console.Write("Enter ID of the test to delete: ");
                    int id = GetIntFromUserInput();
                    Console.WriteLine(_testService.DeleteTest(id) ? "Success!" : "Fail!");
                    break;
                }
                case 5:
                {
                    Console.Write("Enter ID of the test: ");
                    int testId = GetIntFromUserInput();

                    Console.Write("Enter ID of the question: ");
                    int questionId = GetIntFromUserInput();

                    var test = _testService.AddQuestionToTest(testId, questionId);
                    Console.WriteLine("Test info: ");
                    Console.WriteLine($"ID: {test.Id}    Title: {test.Title}    " +
                                      $"Time for each question: {test.TimeForOneQuestion} Questions count: {test.Questions.Count}");
                    break;
                }
                case 6:
                {
                    Console.Write("Enter ID of the test: ");
                    int testId = GetIntFromUserInput();

                    Console.Write("Enter ID of the question: ");
                    int questionId = GetIntFromUserInput();

                    var test = _testService.RemoveQuestionFromTest(testId, questionId);
                    Console.WriteLine("Test info: ");
                    Console.WriteLine($"ID: {test.Id}    Title: {test.Title}    " +
                                      $"Time for each question: {test.TimeForOneQuestion} Questions count: {test.Questions.Count}");
                    break;
                }
                case 7:
                {
                    Console.Write("Enter ID of the test: ");
                    int id = GetIntFromUserInput();

                    Console.Write("Enter new title: ");
                    string title = Console.ReadLine();

                    var test = _testService.UpdateTestTitle(id, title);
                    Console.WriteLine("Test info: ");
                    Console.WriteLine($"ID: {test.Id}    Title: {test.Title}    " +
                                      $"Time for each question: {test.TimeForOneQuestion} Questions count: {test.Questions.Count}");
                    break;
                }
                case 8:
                {
                    Console.Write("Enter ID of the test: ");
                    int id = GetIntFromUserInput();

                    Console.Write("Enter new time: ");
                    int time = GetIntFromUserInput();

                    var test = _testService.UpdateTestTime(id, time);
                    Console.WriteLine("Test info: ");
                    Console.WriteLine($"ID: {test.Id}    Title: {test.Title}    " +
                                      $"Time for each question: {test.TimeForOneQuestion} Questions count: {test.Questions.Count}");
                    break;
                }
                case 9:
                {
                    return;
                }
            }
        }

        private void ShowQuestionsAdminMenu()
        {
            Console.WriteLine();
            Console.WriteLine("List of possible options: ");
            Console.WriteLine("1 View all questions");
            Console.WriteLine("2 Add question");
            Console.WriteLine("3 Delete question");
            Console.WriteLine("4 Add answer to question");
            Console.WriteLine("5 Remove answer from question");
            Console.WriteLine("6 Change question text");
            Console.WriteLine("7 Exit");
            Console.Write("Select option: ");
            switch (GetIntFromUserInput())
            {
                case 1:
                {
                    Console.WriteLine("List of questions: ");
                    var questions = _questionService.GetAllQuestions();
                    foreach (var question in questions)
                    {
                        Console.WriteLine($"ID: {question.Id}    Text: {question.QuestionText}    " +
                                          $"Test ID: {question.TestId}    Test position: {question.PositionInTest}");
                        Console.WriteLine("Answers:");
                        foreach (var answer in question.Answers)
                        {
                            Console.Write($"{answer.PositionInQuestion} {answer.AnswerText}");
                            if (answer.IsCorrect)
                                Console.WriteLine("     +");
                            else
                                Console.WriteLine();
                        }

                        Console.WriteLine();
                    }
                    break;
                }
                case 2:
                {
                    Console.Write("Enter text of the question: ");
                    string text = Console.ReadLine();

                    var question = _questionService.AddQuestion(text);
                    Console.WriteLine($"ID: {question.Id}    Text: {question.QuestionText}    " +
                                      $"Test ID: {question.TestId}    Test position: {question.PositionInTest}");
                    
                    break;
                }
                case 3:
                {
                    Console.Write("Enter ID of the question: ");
                    int id = GetIntFromUserInput();
                    Console.WriteLine(_questionService.DeleteQuestion(id) ? "Success!" : "Fail!");
                    break;
                }
                case 4:
                {
                    Console.Write("Enter ID of the question: ");
                    int questionId = GetIntFromUserInput();

                    Console.Write("Enter text of the answer: ");
                    string text = Console.ReadLine();

                    Console.Write("Is it correct? (Y/n): ");
                    bool isCorrect = Console.ReadLine() == "Y" || Console.ReadLine() == "y";

                    var answer = _answerService.AddAnswer(text, 0, isCorrect);
                    var question = _questionService.AddAnswerToQuestion(questionId, answer.Id);
                    
                    Console.WriteLine($"ID: {question.Id}    Text: {question.QuestionText}    " +
                                      $"Test ID: {question.TestId}    Test position: {question.PositionInTest}");
                    Console.WriteLine("Answers:");
                    foreach (var ans in question.Answers)
                    {
                        Console.Write($"{ans.PositionInQuestion} {ans.AnswerText}");
                        if (ans.IsCorrect)
                            Console.WriteLine("     +");
                        else
                            Console.WriteLine();
                    }
                    
                    break;
                }
                case 5:
                {
                    Console.Write("Enter ID of the question: ");
                    int questionId = GetIntFromUserInput();

                    var question = _questionService.FindQuestionById(questionId);
                    if (question == null)
                    {
                        Console.WriteLine("Wrong ID!");
                        return;
                    }
                    
                    Console.WriteLine($"ID: {question.Id}    Text: {question.QuestionText}    " +
                                      $"Test ID: {question.TestId}    Test position: {question.PositionInTest}");
                    Console.WriteLine("Answers:");
                    foreach (var ans in question.Answers)
                    {
                        Console.Write($"{ans.PositionInQuestion} {ans.AnswerText}");
                        if (ans.IsCorrect)
                            Console.WriteLine("     +");
                        else
                            Console.WriteLine();
                    }


                    Console.Write("Enter number of question: ");
                    var answerId = GetIntFromUserInput();
                    var answer = question.Answers.FirstOrDefault(a => a.PositionInQuestion == answerId);
                    if (answer == null)
                    {
                        Console.WriteLine("Wrong number!");
                        return;
                    }

                    question = _questionService.RemoveAnswerFromQuestion(question.Id, answer.Id);
                    Console.WriteLine($"ID: {question.Id}    Text: {question.QuestionText}    " +
                                      $"Test ID: {question.TestId}    Test position: {question.PositionInTest}");
                    Console.WriteLine("Answers:");
                    foreach (var ans in question.Answers)
                    {
                        Console.Write($"{ans.PositionInQuestion} {ans.AnswerText}");
                        if (ans.IsCorrect)
                            Console.WriteLine("     +");
                        else
                            Console.WriteLine();
                    }
                    
                    break;
                }
                case 6:
                {
                    Console.Write("Enter ID of the question: ");
                    int questionId = GetIntFromUserInput();

                    Console.Write("Enter new text of the question: ");
                    string text = Console.ReadLine();

                    var question = _questionService.UpdateQuestionText(questionId, text);
                    Console.WriteLine($"ID: {question.Id}    Text: {question.QuestionText}    " +
                                      $"Test ID: {question.TestId}    Test position: {question.PositionInTest}");
                    break;
                }
                case 7:
                {
                    return;
                }
            }
        }
        
        private void ShowAnswersAdminMenu()
        {
            Console.WriteLine();
            Console.WriteLine("List of possible options: ");
            Console.WriteLine("1 View all answers");
            Console.WriteLine("2 Delete answer");
            Console.WriteLine("3 Change answer correctness");
            Console.WriteLine("4 Change answer text");
            Console.WriteLine("5 Exit");
            switch (GetIntFromUserInput())
            {
                case 1:
                {
                    var answers = _answerService.GetAllAnswers();
                    foreach (var answer in answers)
                    {
                        Console.WriteLine($"ID: {answer.Id}     Question ID: {answer.QuestionId}    " +
                                          $"Position in question: {answer.PositionInQuestion}    " +
                                          $"Text: {answer.AnswerText}    Is correct: {answer.IsCorrect}");
                    }
                    break;
                }
                case 2:
                {
                    Console.Write("Enter ID of the answer: ");
                    int id = GetIntFromUserInput();
                    Console.WriteLine(_answerService.DeleteAnswer(id) ? "Success!" : "Fail!");
                    break;
                }
                case 3:
                {
                    Console.Write("Enter ID of the answer: ");
                    int id = GetIntFromUserInput();
                    
                    Console.Write("Is it correct? (Y/n): ");
                    bool isCorrect = Console.ReadLine() == "Y" || Console.ReadLine() == "y";

                    var answer = _answerService.UpdateAnswerCorrectness(id, isCorrect);
                    Console.WriteLine($"ID: {answer.Id}     Question ID: {answer.QuestionId}    " +
                                      $"Position in question: {answer.PositionInQuestion}    " +
                                      $"Text: {answer.AnswerText}    Is correct: {answer.IsCorrect}");
                    
                    break;
                }
                case 4:
                {
                    Console.Write("Enter ID of the answer: ");
                    int id = GetIntFromUserInput();
                    
                    Console.Write("Enter new text of the answer: ");
                    string text = Console.ReadLine();

                    var answer = _answerService.UpdateAnswerText(id, text);
                    Console.WriteLine($"ID: {answer.Id}     Question ID: {answer.QuestionId}    " +
                                      $"Position in question: {answer.PositionInQuestion}    " +
                                      $"Text: {answer.AnswerText}    Is correct: {answer.IsCorrect}");
                    break;
                }
                case 5:
                {
                    return;
                }
            }
        }

        private void ShowStatistics()
        {
            Console.WriteLine();
            Console.WriteLine("Your statistics");
            foreach (var passedTest in _passedTests)
            {
                Console.WriteLine($"Test title: {passedTest.TestTitle}");
                Console.WriteLine($"Total time: {passedTest.TotalTime}");
                Console.WriteLine($"Percentage: {passedTest.Percentage}");
                Console.WriteLine();
            }
        }
    }
}