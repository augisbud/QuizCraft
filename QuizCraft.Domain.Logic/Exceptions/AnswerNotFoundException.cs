namespace QuizCraft.Domain.Logic.Exceptions;

public class AnswerNotFoundException(Guid questionId) : Exception
{
    public override string Message => $"Answer for question with id: {questionId} was not found.";
}