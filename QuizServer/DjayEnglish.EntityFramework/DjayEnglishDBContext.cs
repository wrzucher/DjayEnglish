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

        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<ChatQuiz> ChatQuizzes { get; set; }
        public virtual DbSet<ChatQuizAnswer> ChatQuizAnswers { get; set; }
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<QuizAnswerOption> QuizAnswerOptions { get; set; }
        public virtual DbSet<QuizExample> QuizExamples { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<WordAntonym> WordAntonyms { get; set; }
        public virtual DbSet<WordDefinition> WordDefinitions { get; set; }
        public virtual DbSet<WordMeaningExamples4> WordMeaningExamples4s { get; set; }
        public virtual DbSet<WordSynonym> WordSynonyms { get; set; }
        public virtual DbSet<WordUsage> WordUsages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<ChatQuiz>(entity =>
            {
                entity.HasOne(d => d.Chat)
                    .WithMany(p => p.ChatQuizzes)
                    .HasForeignKey(d => d.ChatId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatQuizzesChatId_ToChats");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.ChatQuizzes)
                    .HasForeignKey(d => d.QuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatQuizzesQuizId_ToQuizzes");
            });

            modelBuilder.Entity<ChatQuizAnswer>(entity =>
            {
                entity.HasOne(d => d.Answer)
                    .WithMany(p => p.ChatQuizAnswers)
                    .HasForeignKey(d => d.AnswerId)
                    .HasConstraintName("FK_ChatQuizAnswersAnswerId_ToQuizAnswerOptions");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsFixedLength(true);

                entity.HasOne(d => d.WordDefinition)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.WordDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizzesWordDefinitionId_ToWordDefinitions");
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
                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuizExamples)
                    .HasForeignKey(d => d.QuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizExamplesQuizId_ToQuizes");

                entity.HasOne(d => d.WordUsage)
                    .WithMany(p => p.QuizExamples)
                    .HasForeignKey(d => d.WordUsageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizExamplesWordUsageId_ToWordUsages");
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.Property(e => e.Word1)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("Word");
            });

            modelBuilder.Entity<WordAntonym>(entity =>
            {
                entity.HasOne(d => d.Word)
                    .WithMany(p => p.WordAntonyms)
                    .HasForeignKey(d => d.WordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WordAntonymsWordId_ToWords");
            });

            modelBuilder.Entity<WordDefinition>(entity =>
            {
                entity.Property(e => e.Definition)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.Property(e => e.SourceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('UNKNOWN 1')");

                entity.HasOne(d => d.Word)
                    .WithMany(p => p.WordDefinitions)
                    .HasForeignKey(d => d.WordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WordDefinitionsWordId_ToWords");
            });

            modelBuilder.Entity<WordSynonym>(entity =>
            {
                entity.HasOne(d => d.SynonymWord)
                    .WithMany(p => p.WordSynonymSynonymWords)
                    .HasForeignKey(d => d.SynonymWordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WordSynonymsSynonymWordId_ToWords");

                entity.HasOne(d => d.Word)
                    .WithMany(p => p.WordSynonymWords)
                    .HasForeignKey(d => d.WordId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WordSynonymsWordId_ToWords");
            });

            modelBuilder.Entity<WordUsage>(entity =>
            {
                entity.Property(e => e.Example)
                    .IsRequired()
                    .HasMaxLength(700);

                entity.HasOne(d => d.WordDefinition)
                    .WithMany(p => p.WordUsages)
                    .HasForeignKey(d => d.WordDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WordUsagesWordDefinitionId_ToWordDefinitions");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
