using Moq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using QuizCraft.Domain.API.Filters;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace QuizCraft.Domain.API.Tests.Unit.FiltersTests;

public class SwaggerPdfResponseOperationFilterTests
{
    [Fact]
    public void Apply_AddsPdfResponse_WhenReturnTypeIsTaskOfIActionResult()
    {
        // Arrange
        var operation = new OpenApiOperation();
        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(m => m.ReturnType).Returns(typeof(Task<IActionResult>));

        var apiDescription = new ApiDescription();
        var schemaRepository = new SchemaRepository();
        var context = new OperationFilterContext(apiDescription, null, schemaRepository, methodInfoMock.Object);

        var filter = new SwaggerPdfResponseOperationFilter();

        // Act
        filter.Apply(operation, context);

        // Assert
        Assert.True(operation.Responses.ContainsKey("200"));
        Assert.Equal("PDF file generated", operation.Responses["200"].Description);
        Assert.True(operation.Responses["200"].Content.ContainsKey("application/pdf"));
        Assert.Equal("string", operation.Responses["200"].Content["application/pdf"].Schema.Type);
        Assert.Equal("binary", operation.Responses["200"].Content["application/pdf"].Schema.Format);
    }

    [Fact]
    public void Apply_AddsPdfResponse_WhenReturnTypeIsTaskOfFileResult()
    {
        // Arrange
        var operation = new OpenApiOperation();
        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(m => m.ReturnType).Returns(typeof(Task<FileResult>));

        var apiDescription = new ApiDescription();
        var schemaRepository = new SchemaRepository();
        var context = new OperationFilterContext(apiDescription, null, schemaRepository, methodInfoMock.Object);

        var filter = new SwaggerPdfResponseOperationFilter();

        // Act
        filter.Apply(operation, context);

        // Assert
        Assert.True(operation.Responses.ContainsKey("200"));
        Assert.Equal("PDF file generated", operation.Responses["200"].Description);
        Assert.True(operation.Responses["200"].Content.ContainsKey("application/pdf"));
        Assert.Equal("string", operation.Responses["200"].Content["application/pdf"].Schema.Type);
        Assert.Equal("binary", operation.Responses["200"].Content["application/pdf"].Schema.Format);
    }

    [Fact]
    public void Apply_DoesNotAddPdfResponse_WhenReturnTypeIsNotTaskOfIActionResultOrFileResult()
    {
        // Arrange
        var operation = new OpenApiOperation();
        var methodInfoMock = new Mock<MethodInfo>();
        methodInfoMock.Setup(m => m.ReturnType).Returns(typeof(Task<string>));

        var apiDescription = new ApiDescription();
        var schemaRepository = new SchemaRepository();
        var context = new OperationFilterContext(apiDescription, null, schemaRepository, methodInfoMock.Object);

        var filter = new SwaggerPdfResponseOperationFilter();

        // Act
        filter.Apply(operation, context);

        // Assert
        Assert.False(operation.Responses.ContainsKey("200"));
    }
}