using Microsoft.AspNetCore.Mvc;
using Moq;
using QuizCraft.Domain.API.Controllers;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;

namespace QuizCraft.Domain.API.Tests.Unit
{
    public class QuizControllerTests
    {
        private readonly Mock<IQuizService> _quizService = new();
        private readonly QuizController _controller;

        public QuizControllerTests()
        {
            _controller = new(_quizService.Object);
        }

        [Fact]
        public async void GenerateQuiz_ReturnsExpected()
        {
            // Arrange
            var expectedOutput = new QuestionDto()
            {
                Text = "What is the capital of France?",
                Answers = new List<AnswerDto>
                {
                    new() { Text = "Paris" },
                    new() { Text = "London" },
                    new() { Text = "Berlin" },
                    new() { Text = "Madrid" }
                }
            };

            var fileProcessingResult = new FileProcessingResultDto
            {
                ProcessedData = "The capital of France is Paris." // Simulated file content
            };

            _quizService
                .Setup(x => x.GenerateQuiz(fileProcessingResult.ProcessedData))
                .ReturnsAsync(expectedOutput);

            // Act
            var response = await _controller.GenerateQuiz(fileProcessingResult);

            // Assert
            var result = Assert.IsType<OkObjectResult>(response.Result);
            var data = Assert.IsAssignableFrom<QuestionDto>(result.Value);
            Assert.Equal(expectedOutput.Text, data.Text);
            Assert.Equal(expectedOutput.Answers.Count, data.Answers.Count);
            Assert.Equal(expectedOutput.Answers[0].Text, data.Answers[0].Text);
        }
    }
}
