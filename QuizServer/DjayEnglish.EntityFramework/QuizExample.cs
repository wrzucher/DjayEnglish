using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class QuizExample
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int? TranslationUnitUsageId { get; set; }
        public string Text { get; set; }

        public virtual Quiz Quiz { get; set; }
        public virtual TranslationUnitUsage TranslationUnitUsage { get; set; }
    }
}
