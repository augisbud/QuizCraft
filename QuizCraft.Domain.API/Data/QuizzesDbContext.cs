using Microsoft.EntityFrameworkCore;
using QuizCraft.Domain.API.Entities;

namespace QuizCraft.Domain.API.Data;

public class QuizzesDbContext(DbContextOptions<QuizzesDbContext> options) : DbContext(options)
{
    public required virtual DbSet<Quiz> Quizzes { get; set; }
    public required virtual DbSet<Question> Questions { get; set; }
    public required virtual DbSet<Answer> Answers { get; set; }
    public required virtual DbSet<QuizAnswerAttempt> QuizAnswerAttempts { get; set; }
}