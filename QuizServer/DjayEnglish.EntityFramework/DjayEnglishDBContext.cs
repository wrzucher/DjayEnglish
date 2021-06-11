using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class DjayEnglishDBContext : DbContext
    {
        public DjayEnglishDBContext()
        {
        }

        public DjayEnglishDBContext(DbContextOptions<DjayEnglishDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<QuizAnswerOption> QuizAnswerOptions { get; set; }
        public virtual DbSet<QuizExample> QuizExamples { get; set; }
        public virtual DbSet<TranslationUnit> TranslationUnits { get; set; }
        public virtual DbSet<TranslationUnitAntonym> TranslationUnitAntonyms { get; set; }
        public virtual DbSet<TranslationUnitDefinition> TranslationUnitDefinitions { get; set; }
        public virtual DbSet<TranslationUnitSynonym> TranslationUnitSynonyms { get; set; }
        public virtual DbSet<TranslationUnitUsage> TranslationUnitUsages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserQuiz> UserQuizzes { get; set; }
        public virtual DbSet<UserQuizAnswer> UserQuizAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.TranslationUnitDefinition)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.TranslationUnitDefinitionId)
                    .HasConstraintName("FK_QuizzesTranslationUnitDefinitionId_ToTranslationUnitDefinitions");
            });

            modelBuilder.Entity<QuizAnswerOption>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuizAnswerOptions)
                    .HasForeignKey(d => d.QuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizAnswerOptionsQuizId_ToQuizes");
            });

            modelBuilder.Entity<QuizExample>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuizExamples)
                    .HasForeignKey(d => d.QuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizExamplesQuizId_ToQuizes");

                entity.HasOne(d => d.TranslationUnitUsage)
                    .WithMany(p => p.QuizExamples)
                    .HasForeignKey(d => d.TranslationUnitUsageId)
                    .HasConstraintName("FK_QuizExamplesTranslationUnitUsageId_ToTranslationUnitUsages");
            });

            modelBuilder.Entity<TranslationUnit>(entity =>
            {
                entity.Property(e => e.Spelling)
                    .IsRequired()
                    .HasMaxLength(40);
            });

            modelBuilder.Entity<TranslationUnitAntonym>(entity =>
            {
                entity.HasOne(d => d.AntonymTranslationUnit)
                    .WithMany(p => p.TranslationUnitAntonymAntonymTranslationUnits)
                    .HasForeignKey(d => d.AntonymTranslationUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslationUnitAntonymsAntonymTranslationUnitId_ToTranslationUnits");

                entity.HasOne(d => d.TranslationUnit)
                    .WithMany(p => p.TranslationUnitAntonymTranslationUnits)
                    .HasForeignKey(d => d.TranslationUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslationUnitAntonymsTranslationUnitId_ToTranslationUnits");
            });

            modelBuilder.Entity<TranslationUnitDefinition>(entity =>
            {
                entity.Property(e => e.Definition)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.Property(e => e.SourceName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.TranslationUnit)
                    .WithMany(p => p.TranslationUnitDefinitions)
                    .HasForeignKey(d => d.TranslationUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslationUnitDefinitionsTranslationUnitId_ToTranslationUnits");
            });

            modelBuilder.Entity<TranslationUnitSynonym>(entity =>
            {
                entity.HasOne(d => d.SynonymTranslationUnit)
                    .WithMany(p => p.TranslationUnitSynonymSynonymTranslationUnits)
                    .HasForeignKey(d => d.SynonymTranslationUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslationUnitSynonymsSynonymTranslationUnitId_ToTranslationUnits");

                entity.HasOne(d => d.TranslationUnit)
                    .WithMany(p => p.TranslationUnitSynonymTranslationUnits)
                    .HasForeignKey(d => d.TranslationUnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslationUnitSynonymsTranslationUnitId_ToTranslationUnits");
            });

            modelBuilder.Entity<TranslationUnitUsage>(entity =>
            {
                entity.Property(e => e.Example)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.HasOne(d => d.TranslationUnitDefinition)
                    .WithMany(p => p.TranslationUnitUsages)
                    .HasForeignKey(d => d.TranslationUnitDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TranslationUnitUsagesTranslationUnitDefinitionId_ToTranslationUnitDefinitions");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasMaxLength(42);
            });

            modelBuilder.Entity<UserQuiz>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(42);

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.UserQuizzes)
                    .HasForeignKey(d => d.QuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserQuizzesQuizId_ToQuizzes");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserQuizzes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserQuizzesUserId_ToUsers");
            });

            modelBuilder.Entity<UserQuizAnswer>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(42);

                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.UserQuizAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK_UserQuizAnswersAnswerId_ToQuizAnswerOptions");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserQuizAnswers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserQuizAnswersUserId_ToUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
