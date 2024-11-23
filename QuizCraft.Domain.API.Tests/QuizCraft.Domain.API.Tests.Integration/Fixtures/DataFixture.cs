using QuizCraft.Domain.API.Constants;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Tests.Integration.Fixtures;

public static class DataFixture
{
    public static readonly Guid Quiz1Id = Guid.NewGuid();
    public static readonly Guid Quiz2Id = Guid.NewGuid();

    public static readonly Guid Question1Id = Guid.NewGuid();
    public static readonly Guid Question2Id = Guid.NewGuid();
    public static readonly Guid Question3Id = Guid.NewGuid();

    public static readonly Guid Answer1Id = Guid.NewGuid();
    public static readonly Guid Answer2Id = Guid.NewGuid();
    public static readonly Guid Answer3Id = Guid.NewGuid();
    public static readonly Guid Answer4Id = Guid.NewGuid();
    public static readonly Guid Answer5Id = Guid.NewGuid();
    public static readonly Guid Answer6Id = Guid.NewGuid();

    public static readonly Guid QuizAttempt1Id = Guid.NewGuid();
    public static readonly Guid QuizAnswerAttempt1Id = Guid.NewGuid();
    public static readonly Guid QuizAnswerAttempt2Id = Guid.NewGuid();

    public static readonly IEnumerable<Quiz> Quizzes =
        [
            new Quiz
            {
                Id = Quiz1Id,
                Title = "Basic Math",
                Category = Category.Art,
                CreatedBy = "admin@example.com",
                Questions =
                [
                    new Question
                    {
                        Id = Question1Id,
                        Text = "What is 2 + 2?",
                        QuizId = Quiz1Id,
                        Answers =
                        [
                            new Answer { Id = Answer1Id, Text = "4", IsCorrect = true, QuestionId = Question1Id },
                            new Answer { Id = Answer2Id, Text = "5", IsCorrect = false, QuestionId = Question1Id }
                        ]
                    },
                    new Question
                    {
                        Id = Question2Id,
                        Text = "What is 10 / 2?",
                        QuizId = Quiz1Id,
                        Answers =
                        [
                            new Answer { Id = Answer3Id, Text = "5", IsCorrect = true, QuestionId = Question2Id },
                            new Answer { Id = Answer4Id, Text = "2", IsCorrect = false, QuestionId = Question2Id }
                        ]
                    }
                ]
            },
            new Quiz
            {
                Id = Quiz2Id,
                Title = "General Knowledge",
                Category = Category.GeneralKnowledge,
                CreatedBy = "user@example.com",
                Questions =
                [
                    new Question
                    {
                        Id = Question3Id,
                        Text = "What is the capital of France?",
                        QuizId = Quiz2Id,
                        Answers =
                        [
                            new Answer { Id = Answer5Id, Text = "Paris", IsCorrect = true, QuestionId = Question3Id },
                            new Answer { Id = Answer6Id, Text = "Rome", IsCorrect = false, QuestionId = Question3Id }
                        ]
                    }
                ]
            }
        ];

    public static readonly IEnumerable<QuizAttempt> QuizAttempts =
        [
            new QuizAttempt
            {
                Id = QuizAttempt1Id,
                QuizId = Quiz1Id,
                UserEmail = "student1@example.com",
                IsCompleted = true,
                QuizAnswerAttempts =
                [
                    new QuizAnswerAttempt
                    {
                        Id = QuizAnswerAttempt1Id,
                        QuizAttemptId = QuizAttempt1Id,
                        QuestionId = Question1Id,
                        AnswerId = Answer1Id
                    },
                    new QuizAnswerAttempt
                    {
                        Id = QuizAnswerAttempt2Id,
                        QuizAttemptId = QuizAttempt1Id,
                        QuestionId = Question2Id,
                        AnswerId = Answer3Id
                    }
                ]
            }
        ];

    public static readonly IEnumerable<QuizAnswerAttempt> QuizAnswerAttempts =
        [
            new QuizAnswerAttempt
            {
                Id = QuizAnswerAttempt1Id,
                QuizAttemptId = QuizAttempt1Id,
                QuestionId = Question1Id,
                AnswerId = Answer1Id
            },
            new QuizAnswerAttempt
            {
                Id = QuizAnswerAttempt2Id,
                QuizAttemptId = QuizAttempt1Id,
                QuestionId = Question2Id,
                AnswerId = Answer3Id
            }
        ];
}