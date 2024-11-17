namespace QuizCraft.Domain.API.Exceptions;

public class QuestionsNotFoundException(Guid quizId) : Exception
{
    public override string Message => $"Questions for Quiz with id: {quizId} were not found.";
}