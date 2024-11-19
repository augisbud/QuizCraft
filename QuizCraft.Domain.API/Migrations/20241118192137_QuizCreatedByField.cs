using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizCraft.Domain.API.Migrations
{
    /// <inheritdoc />
    public partial class QuizCreatedByField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Quizzes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Quizzes");
        }
    }
}
