using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class TranslationUnitAntonym
    {
        public int Id { get; set; }
        public int TranslationUnitId { get; set; }
        public int AntonymTranslationUnitId { get; set; }
        public bool IsActive { get; set; }

        public virtual TranslationUnit AntonymTranslationUnit { get; set; }
        public virtual TranslationUnit TranslationUnit { get; set; }
    }
}
