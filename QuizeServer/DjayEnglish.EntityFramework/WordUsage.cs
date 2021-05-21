using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class WordUsage
    {
        public WordUsage()
        {
            QuizExamples = new HashSet<QuizExample>();
        }

        public int Id { get; set; }
        public int WordDefinitionId { get; set; }
        public string Example { get; set; }
        public bool IsActive { get; set; }

        public virtual WordDefinition WordDefinition { get; set; }
        public virtual ICollection<QuizExample> QuizExamples { get; set; }
    }
}
