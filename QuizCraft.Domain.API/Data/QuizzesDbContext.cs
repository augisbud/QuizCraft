using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Data;

public class QuizzesDbContext(DbContextOptions<QuizzesDbContext> options) : DbContext(options)
{
    public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
    public virtual DbSet<Question> Questions { get; set; } = null!;
    public virtual DbSet<Answer> Answers { get; set; } = null!;
    public virtual DbSet<QuizAttempt> QuizAttempts { get; set; } = null!;
    public virtual DbSet<QuizAnswerAttempt> QuizAnswerAttempts { get; set; } = null!;
}