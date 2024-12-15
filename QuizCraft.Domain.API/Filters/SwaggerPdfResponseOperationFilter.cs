using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace QuizCraft.Domain.API.Filters;

public class SwaggerPdfResponseOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var returnType = context.MethodInfo.ReturnType;
        if (returnType == typeof(Task<IActionResult>) || returnType == typeof(Task<FileResult>))
        {
            var content = new OpenApiMediaType
            {
                Schema = new OpenApiSchema { Type = "string", Format = "binary" }
            };

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "PDF file generated",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    { "application/pdf", content }
                }
            };
        }
    }
}