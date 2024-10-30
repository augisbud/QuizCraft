using System;

namespace QuizCraft.Domain.API.Entities
{
    public class QuizAnswerAttempt
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid QuizId { get; set; }
        public Guid QuestionId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string AttemptedAnswer { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;

        public Quiz Quiz { get; set; } = null!;
        public Question Question { get; set; } = null!;
    }
}
