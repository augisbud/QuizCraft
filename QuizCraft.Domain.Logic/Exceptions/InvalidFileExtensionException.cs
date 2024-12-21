namespace QuizCraft.Domain.Logic.Exceptions;

public class InvalidFileExtensionException(string extension) : Exception
{
    public override string Message => $"The provided file extension: '{extension}' is not supported.";
}