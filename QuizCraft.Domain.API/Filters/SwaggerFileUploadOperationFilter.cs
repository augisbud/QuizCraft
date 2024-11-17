using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QuizCraft.Domain.API.Filters;

public class SwaggerFileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasFileUpload = context.ApiDescription.ParameterDescriptions
            .Any(p => p.Type == typeof(IFormFile));

        if (hasFileUpload)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "file",
                In = ParameterLocation.Query,
                Required = true,
                Schema = new OpenApiSchema { Type = "string", Format = "binary" }
            });
        }
    }
}
