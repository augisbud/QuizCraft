﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using QuizCraft.Domain.API.Data;

#nullable disable

namespace QuizCraft.Domain.API.Migrations
{
    [DbContext(typeof(QuizzesDbContext))]
    partial class QuizzesDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("QuizId")
                        .HasColumnType("uuid");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.Quiz", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Category")
                        .HasColumnType("integer");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Quizzes");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.QuizAnswerAttempt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AnswerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("QuestionId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("QuizAttemptId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AnswerId");

                    b.HasIndex("QuestionId");

                    b.HasIndex("QuizAttemptId");

                    b.ToTable("QuizAnswerAttempts");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.QuizAttempt", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("QuizId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserEmail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("QuizId");

                    b.ToTable("QuizAttempts");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.Answer", b =>
                {
                    b.HasOne("QuizCraft.Domain.API.Entities.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.Question", b =>
                {
                    b.HasOne("QuizCraft.Domain.API.Entities.Quiz", "Quiz")
                        .WithMany("Questions")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.QuizAnswerAttempt", b =>
                {
                    b.HasOne("QuizCraft.Domain.API.Entities.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizCraft.Domain.API.Entities.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuizCraft.Domain.API.Entities.QuizAttempt", "QuizAttempt")
                        .WithMany("QuizAnswerAttempts")
                        .HasForeignKey("QuizAttemptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Answer");

                    b.Navigation("Question");

                    b.Navigation("QuizAttempt");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.QuizAttempt", b =>
                {
                    b.HasOne("QuizCraft.Domain.API.Entities.Quiz", "Quiz")
                        .WithMany("QuizAttempts")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.Quiz", b =>
                {
                    b.Navigation("Questions");

                    b.Navigation("QuizAttempts");
                });

            modelBuilder.Entity("QuizCraft.Domain.API.Entities.QuizAttempt", b =>
                {
                    b.Navigation("QuizAnswerAttempts");
                });
#pragma warning restore 612, 618
        }
    }
}
