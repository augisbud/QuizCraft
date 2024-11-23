using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QuizCraft.Domain.API.Entities;
using QuizCraft.Domain.API.Exceptions;

namespace QuizCraft.Domain.API.Handlers;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();

        if (
            exception is AnswerNotFoundException || 
            exception is InsufficientDataException || 
            exception is InvalidFileExtensionException ||
            exception is QuizNotFoundException || 
            exception is QuestionsNotFoundException
        )
        {
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Title = exception.Message;
        }
        else
        {
            logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

            problemDetails.Status = StatusCodes.Status500InternalServerError;
            problemDetails.Title = "Internal Server Error";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}