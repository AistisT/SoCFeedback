using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SoCFeedback.Models;
using SoCFeedback.Enums;

namespace SoCFeedback.Services
{
    public partial class FeedbackDbContext : DbContext
    {
        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Level> Level { get; set; }
        public virtual DbSet<Module> Module { get; set; }
        public virtual DbSet<ModuleQuestions> ModuleQuestions { get; set; }
        public virtual DbSet<PossibleAnswer> PossibleAnswer { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<Year> Year { get; set; }
        public virtual DbSet<YearModules> YearModules { get; set; }

        public FeedbackDbContext(DbContextOptions<FeedbackDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.Id);

                entity.Property(e => e.Answer1)
                    .IsRequired()
                    .HasColumnName("Answer")
                    .HasMaxLength(Constants.AnswerLength);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Answer_Question");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Title).HasName("PK_Title");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.CategoryTitleLength);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.CategoryDescriptionLength);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status");
            });

            modelBuilder.Entity<Supervisor>(entity =>
            {
                entity.HasKey(e => new { e.Forename, e.Surname })
                    .HasName("PK_Supervisor");

                entity.Property(e => e.Title).IsRequired();

                entity.Property(e => e.Forename).IsRequired().HasMaxLength(Constants.NameLength);

                entity.Property(e => e.Surname).IsRequired().HasMaxLength(Constants.NameLength);

                entity.Property(e => e.Email).HasMaxLength(Constants.EmailLength);

                entity.Property(e => e.PhoneNr).HasMaxLength(Constants.PhoneNrLength);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status");
            });

            modelBuilder.Entity<Level>(entity =>
            {
                entity.HasKey(e => e.Title).HasName("PK_Title");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.LevelTitleLength);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PK_Module");

                entity.Property(e => e.Code).HasMaxLength(Constants.ModuleCodeLength);

                entity.Property(e => e.Description).HasMaxLength(Constants.ModuleDescLength);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.ModuleTitleLength);

                entity.Property(e => e.Url)
                    .HasColumnName("URL")
                    .HasMaxLength(Constants.UrlLength);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status");

            entity.Property(e => e.LevelId)
                    .IsRequired()
                    .HasMaxLength(Constants.LevelTitleLength);

                entity.Property(e => e.SupervisorForename).IsRequired().HasMaxLength(Constants.NameLength);

                entity.Property(e => e.SupervisorSurname).IsRequired().HasMaxLength(Constants.NameLength);

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Module)
                    .HasForeignKey(d => d.LevelId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Module_Level");

                entity.HasOne(d => d.Supervisor)
                    .WithMany(p => p.Module)
                    .HasForeignKey(d => new { d.SupervisorForename, d.SupervisorSurname })
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Module_Supervisor");
            });

            modelBuilder.Entity<ModuleQuestions>(entity =>
            {
                entity.HasKey(e => new { e.ModuleCode, e.QuestionId })
                    .HasName("PK_ModuleQuestions");

                entity.Property(e => e.ModuleCode).HasMaxLength(Constants.ModuleCodeLength);

                entity.HasOne(d => d.ModuleCodeNavigation)
                    .WithMany(p => p.ModuleQuestions)
                    .HasForeignKey(d => d.ModuleCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ModuleQuestions_Module");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ModuleQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_ModuleQuestions_Question");
            });

            modelBuilder.Entity<PossibleAnswer>(entity =>
            {
                entity.Property(e => e.Id);

                entity.Property(e => e.Answer)
                    .IsRequired()
                    .HasMaxLength(Constants.PossibleAnswerLength);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.PossibleAnswer)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_PossibleAnswer_Question");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.Id);

                entity.Property(e => e.Optional).HasDefaultValueSql("0");

                entity.Property(e => e.Question1)
                    .IsRequired()
                    .HasColumnName("Question")
                    .HasMaxLength(Constants.QuestionLength);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("Status");

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(Constants.CategoryTitleLength);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Question_Category");
            });

            modelBuilder.Entity<Year>(entity =>
            {
                entity.HasKey(e => e.Year1)
                    .HasName("PK_Year");

                entity.Property(e => e.Year1)
                    .HasColumnName("Year")
                    .ValueGeneratedNever();

                entity.Property(e => e.Status)
                    .HasColumnName("Status")
                    .IsRequired();
            });

            modelBuilder.Entity<YearModules>(entity =>
            {
                entity.HasKey(e => new { e.Year, e.ModuleCode })
                    .HasName("PK_YearModules");

                entity.Property(e => e.ModuleCode).HasMaxLength(Constants.ModuleCodeLength);

                entity.HasOne(d => d.ModuleCodeNavigation)
                    .WithMany(p => p.YearModules)
                    .HasForeignKey(d => d.ModuleCode)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_YearModules_Module");

                entity.HasOne(d => d.YearNavigation)
                    .WithMany(p => p.YearModules)
                    .HasForeignKey(d => d.Year)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_YearModules_Year");
            });
        }
    }
}