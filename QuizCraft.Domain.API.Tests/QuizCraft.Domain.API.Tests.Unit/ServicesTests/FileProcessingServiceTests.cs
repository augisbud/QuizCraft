using System.Text;
using Moq;
using Microsoft.AspNetCore.Http;
using QuizCraft.Domain.API.Services;
using QuizCraft.Domain.API.Exceptions;
using Xceed.Words.NET;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace QuizCraft.Domain.API.Tests.Unit.ServicesTests;

public class FileProcessingServiceTests
{
    private readonly FileProcessingService _fileProcessingService;

    public FileProcessingServiceTests()
    {
        _fileProcessingService = new FileProcessingService();
    }

    [Fact]
    public async Task ProcessFileAsync_ProcessesTextFile()
    {
        // Arrange
        var content = "This is a text file.";
        var fileName = "sample.txt";
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(content));
        ms.Position = 0;
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.CopyTo(It.IsAny<Stream>())).Callback<Stream>(s =>
        {
            ms.CopyTo(s);
            s.Position = 0;
        });
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.Length).Returns(ms.Length);

        // Act
        var result = await _fileProcessingService.ProcessFileAsync(fileMock.Object);

        // Assert
        Assert.Equal(content, result);
    }

    [Fact]
    public async Task ProcessFileAsync_ProcessesDocxFile()
    {
        // Arrange
        var content = "This is a sample .docx file.";
        var fileName = "sample.docx";
        var ms = new MemoryStream();
        using (var document = DocX.Create(ms))
        {
            document.InsertParagraph(content);
            document.Save();
        }
        ms.Position = 0;
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.CopyTo(It.IsAny<Stream>())).Callback<Stream>(s =>
        {
            ms.CopyTo(s);
            s.Position = 0;
        });
        fileMock.Setup(_ => _.FileName).Returns(fileName);

        // Act
        var result = await _fileProcessingService.ProcessFileAsync(fileMock.Object);

        // Assert
        Assert.Equal(content, result.Trim());
    }

    [Fact]
    public async Task ProcessFileAsync_ProcessesPdfFile()
    {
        // Arrange
        var content = "This is a sample PDF file.";
        var fileName = "sample.pdf";

        var ms = new MemoryStream();
        var writer = new PdfWriter(ms);
        writer.SetCloseStream(false);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);
        document.Add(new Paragraph(content));
        document.Close();
        ms.Position = 0;

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.CopyTo(It.IsAny<Stream>())).Callback<Stream>(s =>
        {
            ms.Position = 0;
            ms.CopyTo(s);
            s.Position = 0;
        });
        fileMock.Setup(_ => _.FileName).Returns(fileName);
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);

        // Act
        var result = await _fileProcessingService.ProcessFileAsync(fileMock.Object);

        // Assert
        Assert.Contains(content, result);
    }

    [Fact]
    public async Task ProcessFileAsync_ThrowsInvalidFileExtensionException()
    {
        // Arrange
        var fileName = "test.invalid";
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(_ => _.FileName).Returns(fileName);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidFileExtensionException>(() => _fileProcessingService.ProcessFileAsync(fileMock.Object));
    }
}