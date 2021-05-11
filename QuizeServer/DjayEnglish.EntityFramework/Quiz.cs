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
            QuizeAnswerOptions = new HashSet<QuizeAnswerOption>();
            QuizeExamples = new HashSet<QuizeExample>();
        }

        public int Id { get; set; }
        public int WordDefinitionId { get; set; }
        public string Question { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Closed { get; set; }

        public virtual WordDefinition WordDefinition { get; set; }
        public virtual ICollection<ChatQuiz> ChatQuizzes { get; set; }
        public virtual ICollection<QuizeAnswerOption> QuizeAnswerOptions { get; set; }
        public virtual ICollection<QuizeExample> QuizeExamples { get; set; }
    }
}
