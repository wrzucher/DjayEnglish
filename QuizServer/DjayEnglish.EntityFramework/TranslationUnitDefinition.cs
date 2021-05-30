using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class TranslationUnitDefinition
    {
        public TranslationUnitDefinition()
        {
            Quizzes = new HashSet<Quiz>();
            TranslationUnitUsages = new HashSet<TranslationUnitUsage>();
        }

        public int Id { get; set; }
        public int TranslationUnitId { get; set; }
        public string Definition { get; set; }
        public bool IsActive { get; set; }
        public int SourceId { get; set; }
        public string SourceName { get; set; }

        public virtual TranslationUnit TranslationUnit { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<TranslationUnitUsage> TranslationUnitUsages { get; set; }
    }
}
