namespace QuizCraft.Domain.API.Exceptions;

public class InsufficientDataException : Exception
{
    public override string Message => $"The provided source file did not yield sufficient data to generate a quiz.";
}