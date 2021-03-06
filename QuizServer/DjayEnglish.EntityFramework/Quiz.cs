using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class Quiz
    {
        public Quiz()
        {
            QuizAnswerOptions = new HashSet<QuizAnswerOption>();
            QuizExamples = new HashSet<QuizExample>();
            UserQuizzes = new HashSet<UserQuiz>();
        }

        public int Id { get; set; }
        public int? TranslationUnitDefinitionId { get; set; }
        public string Question { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Closed { get; set; }
        public byte QuestionType { get; set; }
        public byte QuestionShowType { get; set; }
        public byte ExampleShowType { get; set; }
        public byte AnswerShowType { get; set; }
        public bool HasAudioFiles { get; set; }

        public virtual TranslationUnitDefinition TranslationUnitDefinition { get; set; }
        public virtual ICollection<QuizAnswerOption> QuizAnswerOptions { get; set; }
        public virtual ICollection<QuizExample> QuizExamples { get; set; }
        public virtual ICollection<UserQuiz> UserQuizzes { get; set; }
    }
}
