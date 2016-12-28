﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SoCFeedback.Services;
using SoCFeedback.Enums;

namespace SoCFeedback.Data.Migrations
{
    [DbContext(typeof(FeedbackDbContext))]
    [Migration("20161227221008_FeedDbV1.2")]
    partial class FeedDbV12
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SoCFeedback.Models.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer1")
                        .IsRequired()
                        .HasColumnName("Answer")
                        .HasMaxLength(500);

                    b.Property<Guid>("QuestionId");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("SoCFeedback.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Status")
                        .HasColumnName("Status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("SoCFeedback.Models.Level", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Status")
                        .HasColumnName("Status");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Level");
                });

            modelBuilder.Entity("SoCFeedback.Models.Module", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nchar(20)");

                    b.Property<string>("Description")
                        .HasMaxLength(500);

                    b.Property<Guid>("LevelId");

                    b.Property<int>("Status")
                        .HasColumnName("Status");

                    b.Property<Guid>("SupervisorId");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("Url")
                        .HasColumnName("URL")
                        .HasMaxLength(250);

                    b.HasKey("Code")
                        .HasName("PK_Module");

                    b.HasIndex("LevelId");

                    b.HasIndex("SupervisorId");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("SoCFeedback.Models.ModuleQuestions", b =>
                {
                    b.Property<string>("ModuleCode")
                        .HasColumnType("nchar(20)");

                    b.Property<Guid>("QuestionId");

                    b.HasKey("ModuleCode", "QuestionId")
                        .HasName("PK_ModuleQuestions");

                    b.HasIndex("QuestionId");

                    b.ToTable("ModuleQuestions");
                });

            modelBuilder.Entity("SoCFeedback.Models.PossibleAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<Guid>("QuestionId");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.ToTable("PossibleAnswer");
                });

            modelBuilder.Entity("SoCFeedback.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CategoryId");

                    b.Property<bool>("Optional")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("0");

                    b.Property<string>("Question1")
                        .IsRequired()
                        .HasColumnName("Question")
                        .HasMaxLength(250);

                    b.Property<int>("Section");

                    b.Property<int>("Status")
                        .HasColumnName("Status");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("SoCFeedback.Models.Supervisor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .HasMaxLength(50);

                    b.Property<string>("Forename")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PhoneNr")
                        .HasMaxLength(20);

                    b.Property<int>("Status")
                        .HasColumnName("Status");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Title");

                    b.HasKey("Id");

                    b.ToTable("Supervisor");
                });

            modelBuilder.Entity("SoCFeedback.Models.Year", b =>
                {
                    b.Property<int>("Year1")
                        .HasColumnName("Year");

                    b.Property<int>("Status")
                        .HasColumnName("Status");

                    b.HasKey("Year1")
                        .HasName("PK_Year");

                    b.ToTable("Year");
                });

            modelBuilder.Entity("SoCFeedback.Models.YearModules", b =>
                {
                    b.Property<int>("Year");

                    b.Property<string>("ModuleCode")
                        .HasColumnType("nchar(20)");

                    b.HasKey("Year", "ModuleCode")
                        .HasName("PK_YearModules");

                    b.HasIndex("ModuleCode");

                    b.ToTable("YearModules");
                });

            modelBuilder.Entity("SoCFeedback.Models.Answer", b =>
                {
                    b.HasOne("SoCFeedback.Models.Question", "Question")
                        .WithMany("Answer")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("FK_Answer_Question");
                });

            modelBuilder.Entity("SoCFeedback.Models.Module", b =>
                {
                    b.HasOne("SoCFeedback.Models.Level", "Level")
                        .WithMany("Module")
                        .HasForeignKey("LevelId")
                        .HasConstraintName("FK_Module_Level");

                    b.HasOne("SoCFeedback.Models.Supervisor", "Supervisor")
                        .WithMany("Module")
                        .HasForeignKey("SupervisorId")
                        .HasConstraintName("FK_Module_Supervisor");
                });

            modelBuilder.Entity("SoCFeedback.Models.ModuleQuestions", b =>
                {
                    b.HasOne("SoCFeedback.Models.Module", "ModuleCodeNavigation")
                        .WithMany("ModuleQuestions")
                        .HasForeignKey("ModuleCode")
                        .HasConstraintName("FK_ModuleQuestions_Module");

                    b.HasOne("SoCFeedback.Models.Question", "Question")
                        .WithMany("ModuleQuestions")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("FK_ModuleQuestions_Question");
                });

            modelBuilder.Entity("SoCFeedback.Models.PossibleAnswer", b =>
                {
                    b.HasOne("SoCFeedback.Models.Question", "Question")
                        .WithMany("PossibleAnswer")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("FK_PossibleAnswer_Question");
                });

            modelBuilder.Entity("SoCFeedback.Models.Question", b =>
                {
                    b.HasOne("SoCFeedback.Models.Category", "Category")
                        .WithMany("Question")
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("FK_Question_Category");
                });

            modelBuilder.Entity("SoCFeedback.Models.YearModules", b =>
                {
                    b.HasOne("SoCFeedback.Models.Module", "ModuleCodeNavigation")
                        .WithMany("YearModules")
                        .HasForeignKey("ModuleCode")
                        .HasConstraintName("FK_YearModules_Module");

                    b.HasOne("SoCFeedback.Models.Year", "YearNavigation")
                        .WithMany("YearModules")
                        .HasForeignKey("Year")
                        .HasConstraintName("FK_YearModules_Year");
                });
        }
    }
}