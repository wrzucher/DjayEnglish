using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class Quiz
    {
        public Quiz()
        {
            ChatQuizzes = new HashSet<ChatQuiz>();
            QuizAnswerOptions = new HashSet<QuizAnswerOption>();
            QuizExamples = new HashSet<QuizExample>();
        }

        public int Id { get; set; }
        public int WordDefinitionId { get; set; }
        public string Question { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Closed { get; set; }
        public byte QuestionShowType { get; set; }
        public byte ExampleShowType { get; set; }
        public byte AnswerShowType { get; set; }
        public bool HasAudioFiles { get; set; }

        public virtual WordDefinition WordDefinition { get; set; }
        public virtual ICollection<ChatQuiz> ChatQuizzes { get; set; }
        public virtual ICollection<QuizAnswerOption> QuizAnswerOptions { get; set; }
        public virtual ICollection<QuizExample> QuizExamples { get; set; }
    }
}
