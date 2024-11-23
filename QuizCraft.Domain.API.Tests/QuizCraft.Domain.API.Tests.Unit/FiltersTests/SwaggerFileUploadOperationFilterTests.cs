using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Moq;
using QuizCraft.Domain.API.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QuizCraft.Domain.API.Tests.Unit.FiltersTests;

public class SwaggerFileUploadOperationFilterTests
{
    [Fact]
    public void Apply_AddsFileParameter_WhenIFormFileParameterExists()
    {
        // Arrange
        var operation = new OpenApiOperation
        {
            Parameters = []
        };

        var apiParameterDescription = new ApiParameterDescription
        {
            Type = typeof(IFormFile)
        };

        var apiDescription = new ApiDescription();
        apiDescription.ParameterDescriptions.Add(apiParameterDescription);

        var schemaGenerator = new Mock<ISchemaGenerator>();
        var schemaRepository = new SchemaRepository();
        var methodInfo = typeof(SwaggerFileUploadOperationFilterTests).GetMethod(nameof(Apply_AddsFileParameter_WhenIFormFileParameterExists));
        var context = new OperationFilterContext(apiDescription, schemaGenerator.Object, schemaRepository, methodInfo);

        var filter = new SwaggerFileUploadOperationFilter();

        // Act
        filter.Apply(operation, context);

        // Assert
        Assert.Single(operation.Parameters);
        var parameter = operation.Parameters.First();
        Assert.Equal("file", parameter.Name);
        Assert.Equal(ParameterLocation.Query, parameter.In);
        Assert.True(parameter.Required);
        Assert.Equal("string", parameter.Schema.Type);
        Assert.Equal("binary", parameter.Schema.Format);
    }

    [Fact]
    public void Apply_DoesNotAddFileParameter_WhenIFormFileParameterDoesNotExist()
    {
        // Arrange
        var operation = new OpenApiOperation
        {
            Parameters = []
        };

        var apiParameterDescription = new ApiParameterDescription
        {
            Type = typeof(string)
        };

        var apiDescription = new ApiDescription();
        apiDescription.ParameterDescriptions.Add(apiParameterDescription);

        var schemaGenerator = new Mock<ISchemaGenerator>();
        var schemaRepository = new SchemaRepository();
        var methodInfo = typeof(SwaggerFileUploadOperationFilterTests).GetMethod(nameof(Apply_DoesNotAddFileParameter_WhenIFormFileParameterDoesNotExist));
        var context = new OperationFilterContext(apiDescription, schemaGenerator.Object, schemaRepository, methodInfo);

        var filter = new SwaggerFileUploadOperationFilter();

        // Act
        filter.Apply(operation, context);

        // Assert
        Assert.Empty(operation.Parameters);
    }
}