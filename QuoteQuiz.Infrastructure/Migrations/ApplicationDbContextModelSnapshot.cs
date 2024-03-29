﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QuoteQuiz.Infrastructure.Database;

#nullable disable

namespace QuoteQuiz.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QuoteQuiz.Application.Domain.Quiz", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Published")
                        .HasColumnType("bit")
                        .HasColumnName("Published");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("Title");

                    b.HasKey("Id")
                        .HasName("Pk_Quiz_Id");

                    b.ToTable("Quiz", (string)null);
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Answers")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Answers");

                    b.Property<int>("CorectAnswer")
                        .HasColumnType("int");

                    b.Property<int>("QuizId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Text");

                    b.HasKey("Id")
                        .HasName("Pk_Quote_Id");

                    b.HasIndex("QuizId");

                    b.ToTable("Quote");
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2")
                        .HasColumnName("Created");

                    b.Property<DateTime>("Expires")
                        .HasColumnType("datetime2")
                        .HasColumnName("Expires");

                    b.Property<int?>("Fk_User_Id")
                        .HasColumnType("int");

                    b.Property<string>("TokenHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TokenHash");

                    b.HasKey("Id")
                        .HasName("Pk_RefreshToken_Id");

                    b.HasIndex("Fk_User_Id");

                    b.ToTable("RefreshToken", (string)null);
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("Name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("PasswordHash");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)")
                        .HasColumnName("Salt");

                    b.HasKey("Id")
                        .HasName("Pk_User_Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.UserAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Answer")
                        .HasColumnType("int")
                        .HasColumnName("UserAnswer");

                    b.Property<DateTime>("OnDate")
                        .HasColumnType("datetime2")
                        .HasColumnName("OnDate");

                    b.Property<bool?>("QuizFinished")
                        .HasColumnType("bit")
                        .HasColumnName("QuizFinished");

                    b.Property<int>("QuizId")
                        .HasColumnType("int")
                        .HasColumnName("Fk_Quiz_Id");

                    b.Property<int?>("QuoteId")
                        .HasColumnType("int")
                        .HasColumnName("Fk_Quote_Id");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("Fk_User_Id");

                    b.HasKey("Id")
                        .HasName("Pk_UserAnswer_Id");

                    b.HasIndex("QuizId");

                    b.HasIndex("QuoteId");

                    b.HasIndex("UserId");

                    b.ToTable("UserAnswer", (string)null);
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.Quote", b =>
                {
                    b.HasOne("QuoteQuiz.Application.Domain.Quiz", null)
                        .WithMany("Quotes")
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.RefreshToken", b =>
                {
                    b.HasOne("QuoteQuiz.Application.Domain.User", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("Fk_User_Id");
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.UserAnswer", b =>
                {
                    b.HasOne("QuoteQuiz.Application.Domain.Quiz", "Quiz")
                        .WithMany()
                        .HasForeignKey("QuizId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QuoteQuiz.Application.Domain.Quote", "Quote")
                        .WithMany()
                        .HasForeignKey("QuoteId");

                    b.HasOne("QuoteQuiz.Application.Domain.User", null)
                        .WithMany("Answers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quiz");

                    b.Navigation("Quote");
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.Quiz", b =>
                {
                    b.Navigation("Quotes");
                });

            modelBuilder.Entity("QuoteQuiz.Application.Domain.User", b =>
                {
                    b.Navigation("Answers");

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
