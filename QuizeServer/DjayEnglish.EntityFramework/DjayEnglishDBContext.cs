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
        public virtual DbSet<Quiz> Quizzes { get; set; }
        public virtual DbSet<QuizeAnswerOption> QuizeAnswerOptions { get; set; }
        public virtual DbSet<QuizeExample> QuizeExamples { get; set; }
        public virtual DbSet<Word> Words { get; set; }
        public virtual DbSet<WordAntonym> WordAntonyms { get; set; }
        public virtual DbSet<WordDefinition> WordDefinitions { get; set; }
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

                entity.HasOne(d => d.Quize)
                    .WithMany(p => p.ChatQuizzes)
                    .HasForeignKey(d => d.QuizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatQuizzesQuizeId_ToQuizes");
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

            modelBuilder.Entity<QuizeAnswerOption>(entity =>
            {
                entity.Property(e => e.ShowedKey)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.HasOne(d => d.Quize)
                    .WithMany(p => p.QuizeAnswerOptions)
                    .HasForeignKey(d => d.QuizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizeAnswerOptionsQuizeId_ToQuizes");
            });

            modelBuilder.Entity<QuizeExample>(entity =>
            {
                entity.HasOne(d => d.Quize)
                    .WithMany(p => p.QuizeExamples)
                    .HasForeignKey(d => d.QuizeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizeExamplesQuizeId_ToQuizes");

                entity.HasOne(d => d.WordUsages)
                    .WithMany(p => p.QuizeExamples)
                    .HasForeignKey(d => d.WordUsagesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QuizeExamplesWordUsagesId_ToWordUsages");
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
