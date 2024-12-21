namespace QuizCraft.Domain.Logic.Exceptions;

public class QuizNotFoundException(Guid id) : Exception
{
    public override string Message => $"Quiz with id: {id} was not found.";
}