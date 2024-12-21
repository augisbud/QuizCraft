namespace QuizCraft.Domain.Logic.Exceptions;

public class MissingConfigurationException(string property) : Exception
{
    public override string Message => $"Property: {property} is missing from runtime configuration.";
}