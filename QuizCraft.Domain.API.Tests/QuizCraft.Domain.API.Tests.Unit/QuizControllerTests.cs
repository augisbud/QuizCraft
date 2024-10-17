using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using QuizCraft.Domain.API.Controllers;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizCraft.Domain.API.Tests.Unit
{
    public class QuizControllerTests
    {
        private readonly Mock<IQuizService> _quizServiceMock = new();
        private readonly Mock<IFileProcessingService> _fileProcessingServiceMock = new();
        private readonly QuizController _controller;

        public QuizControllerTests()
        {
            _controller = new QuizController(_quizServiceMock.Object, _fileProcessingServiceMock.Object);
        }

        [Fact]
        public async Task RetrieveQuizzes_ValidFile_ReturnsGeneratedQuiz()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.docx");
            mockFile.Setup(f => f.Length).Returns(1024);

            var processedData = "Processed file content";
            _fileProcessingServiceMock.Setup(s => s.ProcessFileAsync(It.IsAny<IFormFile>()))
                                      .ReturnsAsync(processedData);

            var expectedQuiz = new QuizDto
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                Questions = new List<QuestionDto>
                {
                    new QuestionDto
                    {
                        Text = "What is the capital of France?",
                        Answers = new List<AnswerDto>
                        {
                            new AnswerDto("Paris", true),
                            new AnswerDto("London", false),
                            new AnswerDto("Berlin", false),
                            new AnswerDto("Madrid", false)
                        }
                    }
                }
            };

            _quizServiceMock.Setup(s => s.CreateQuizAsync(processedData))
                            .ReturnsAsync(expectedQuiz);

            // Act
            var result = await _controller.CreateQuizAsync(mockFile.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuiz = Assert.IsAssignableFrom<QuizDto>(okResult.Value);
            var expectedGuid = returnedQuiz.Id;
            Assert.Equal(expectedGuid.ToString(), returnedQuiz.Id.ToString());
            Assert.NotNull(returnedQuiz.Questions);
            Assert.Single(returnedQuiz.Questions);
            Assert.Equal("What is the capital of France?", returnedQuiz.Questions.First().Text);
        }

        [Fact]
        public async Task CreateQuizAsync_InvalidFileType_ReturnsBadRequest()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.exe");
            mockFile.Setup(f => f.Length).Returns(1024);

            // Act
            var result = await _controller.CreateQuizAsync(mockFile.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid file type. Only .txt, .docx, and .pdf files are allowed.", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateQuizAsync_EmptyFile_ReturnsBadRequest()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.docx");
            mockFile.Setup(f => f.Length).Returns(0);

            // Act
            var result = await _controller.CreateQuizAsync(mockFile.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("File processing failed.", badRequestResult.Value);
        }
    }
}
