using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Tests.Integration.Fixtures;

public static class DataFixture
{
    public static readonly IEnumerable<Quiz> Quizzes =
    [
        new Quiz
        {
            Id = Guid.NewGuid(),
            Questions =
            [
                new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "What is 2 + 2?",
                    Answers =
                    [
                        new() 
                        {
                            Id = Guid.NewGuid(),
                            Text = "4",
                        },
                        new() 
                        {
                            Id = Guid.NewGuid(),
                            Text = "5",
                        }
                    ]
                }
            ]
        }
    ];
}