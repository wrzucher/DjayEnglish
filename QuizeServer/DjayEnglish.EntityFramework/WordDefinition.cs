using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class WordDefinition
    {
        public WordDefinition()
        {
            Quizzes = new HashSet<Quiz>();
            WordExamples = new HashSet<WordExample>();
        }

        public int Id { get; set; }
        public int WordId { get; set; }
        public string Definition { get; set; }
        public bool IsActive { get; set; }
        public int SourceId { get; set; }
        public string SourceName { get; set; }

        public virtual Word Word { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<WordExample> WordExamples { get; set; }
    }
}
