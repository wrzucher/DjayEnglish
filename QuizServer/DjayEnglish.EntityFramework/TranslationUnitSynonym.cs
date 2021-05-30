using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class TranslationUnitSynonym
    {
        public int Id { get; set; }
        public int TranslationUnitId { get; set; }
        public int SynonymTranslationUnitId { get; set; }
        public bool IsActive { get; set; }

        public virtual TranslationUnit SynonymTranslationUnit { get; set; }
        public virtual TranslationUnit TranslationUnit { get; set; }
    }
}
