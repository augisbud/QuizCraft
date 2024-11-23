namespace QuizCraft.Domain.API.Exceptions;

public class MissingConfigurationException(string property) : Exception
{
    public override string Message => $"Property: {property} is missing from runtime configuration.";
}