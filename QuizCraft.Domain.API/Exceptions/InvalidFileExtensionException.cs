namespace QuizCraft.Domain.API.Exceptions;

public class InvalidFileExtensionException(string extension) : Exception
{
    public override string Message => $"The provided file extension: '{extension}' is not supported.";
}