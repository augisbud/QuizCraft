using Newtonsoft.Json;
using QuizCraft.Domain.API.Constants;
using QuizCraft.Domain.API.Models;

namespace QuizCraft.Tests.Domain.API.Constants;

public class GeminiAPITemplatesTests
{
    [Fact]
    public void GeneratePrompt_IncludesSourceInOutput()
    {
        // Arrange
        string source = "Sample source text";

        // Act
        string result = GeminiAPITemplates.GeneratePrompt(source);

        // Assert
        Assert.Contains($"The source is '{source}'", result);
    }

    [Fact]
    public void GeneratePrompt_IncludesSerializedExampleQuestion()
    {
        // Arrange
        var exampleQuestion = new QuizDtoForCreation
        {
            Title = "Come up with a title",
            Category = Category.GeneralKnowledge,
            Questions =
                [
                    new QuestionForCreationDto
                    {
                        Text = "The question you came up with.",
                        Answers =
                        [
                            new AnswerForCreationDto("Correct answer", true),
                            new AnswerForCreationDto("Incorrect answer", false),
                        ]
                    }
                ]
        };

        string serializedExampleQuestion = JsonConvert.SerializeObject(exampleQuestion);

        // Act
        string result = GeminiAPITemplates.GeneratePrompt("Sample source text");

        // Assert
        Assert.Contains(serializedExampleQuestion, result);
    }
}