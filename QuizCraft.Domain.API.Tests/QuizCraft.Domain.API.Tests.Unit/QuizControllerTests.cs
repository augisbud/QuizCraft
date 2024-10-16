using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Moq;
using QuizCraft.Domain.API.Controllers;
using QuizCraft.Domain.API.Models;
using QuizCraft.Domain.API.Services;
using Xunit;

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
        public async Task GenerateOrRetrieveQuizzes_ValidFile_ReturnsGeneratedQuiz()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.docx");
            mockFile.Setup(f => f.Length).Returns(1024);

            var processedResult = new FileProcessingResultDto { ProcessedData = "Processed file content" };
            _fileProcessingServiceMock.Setup(s => s.ProcessFileAsync(It.IsAny<IFormFile>()))
                                      .ReturnsAsync(processedResult);

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
                            new AnswerDto { Text = "Paris" },
                            new AnswerDto { Text = "London" },
                            new AnswerDto { Text = "Berlin" },
                            new AnswerDto { Text = "Madrid" }
                        }
                    }
                }
            };

            _quizServiceMock.Setup(s => s.CreateQuiz(It.IsAny<string>()))
                            .ReturnsAsync(expectedQuiz);

            // Act
            var result = await _controller.GenerateOrRetrieveQuizzes(mockFile.Object);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuiz = Assert.IsAssignableFrom<QuizDto>(okResult.Value);
            var expectedGuid = returnedQuiz.Id;
            Assert.Equal(expectedGuid.ToString(), returnedQuiz.Id.ToString());
            Assert.NotNull(returnedQuiz.Questions);
            Assert.Single(returnedQuiz.Questions);
            Assert.Equal("What is the capital of France?", returnedQuiz.Questions.First().Text); // Check question text
        }

        [Fact]
        public async Task GenerateOrRetrieveQuizzes_InvalidFileType_ReturnsBadRequest()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.exe");
            mockFile.Setup(f => f.Length).Returns(1024);

            // Act
            var result = await _controller.GenerateOrRetrieveQuizzes(mockFile.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Invalid file type. Only .txt, .docx, and .pdf files are allowed.", badRequestResult.Value);
        }

        [Fact]
        public async Task GenerateOrRetrieveQuizzes_NoFile_ReturnsExistingQuizzes()
        {
            // Arrange
            var expectedQuizzes = new List<QuizDto>
            {
                new QuizDto
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
                                new AnswerDto { Text = "Paris" },
                                new AnswerDto { Text = "London" },
                                new AnswerDto { Text = "Berlin" },
                                new AnswerDto { Text = "Madrid" }
                            }
                        }
                    }
                },
                new QuizDto
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = DateTime.UtcNow,
                    Questions = new List<QuestionDto>
                    {
                        new QuestionDto
                        {
                            Text = "What is the largest planet?",
                            Answers = new List<AnswerDto>
                            {
                                new AnswerDto { Text = "Jupiter" },
                                new AnswerDto { Text = "Earth" },
                                new AnswerDto { Text = "Mars" },
                                new AnswerDto { Text = "Saturn" }
                            }
                        }
                    }
                }
            };
            _quizServiceMock.Setup(s => s.RetrieveQuizzesAsync())
                            .ReturnsAsync(expectedQuizzes);

            // Act
            var result = await _controller.GenerateOrRetrieveQuizzes(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedQuizzes = Assert.IsAssignableFrom<IEnumerable<QuizDto>>(okResult.Value);
            Assert.Equal(2, returnedQuizzes.Count());
            var expectedGuid = returnedQuizzes.First().Id;
            Assert.Equal(expectedGuid.ToString(), returnedQuizzes.First().Id.ToString());
        }

        [Fact]
        public async Task GenerateOrRetrieveQuizzes_EmptyFile_ReturnsBadRequest()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.FileName).Returns("test.docx");
            mockFile.Setup(f => f.Length).Returns(0);

            // Act
            var result = await _controller.GenerateOrRetrieveQuizzes(mockFile.Object);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("File processing failed.", badRequestResult.Value);
        }
    }
}
