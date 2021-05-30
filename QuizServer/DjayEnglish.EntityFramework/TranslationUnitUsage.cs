using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class TranslationUnitUsage
    {
        public TranslationUnitUsage()
        {
            QuizExamples = new HashSet<QuizExample>();
        }

        public int Id { get; set; }
        public int TranslationUnitDefinitionId { get; set; }
        public string Example { get; set; }
        public bool IsActive { get; set; }

        public virtual TranslationUnitDefinition TranslationUnitDefinition { get; set; }
        public virtual ICollection<QuizExample> QuizExamples { get; set; }
    }
}
