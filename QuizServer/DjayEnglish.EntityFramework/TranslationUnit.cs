using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class TranslationUnit
    {
        public TranslationUnit()
        {
            TranslationUnitAntonymAntonymTranslationUnits = new HashSet<TranslationUnitAntonym>();
            TranslationUnitAntonymTranslationUnits = new HashSet<TranslationUnitAntonym>();
            TranslationUnitDefinitions = new HashSet<TranslationUnitDefinition>();
            TranslationUnitSynonymSynonymTranslationUnits = new HashSet<TranslationUnitSynonym>();
            TranslationUnitSynonymTranslationUnits = new HashSet<TranslationUnitSynonym>();
        }

        public int Id { get; set; }
        public string Spelling { get; set; }
        public byte PartOfSpeech { get; set; }
        public byte Language { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<TranslationUnitAntonym> TranslationUnitAntonymAntonymTranslationUnits { get; set; }
        public virtual ICollection<TranslationUnitAntonym> TranslationUnitAntonymTranslationUnits { get; set; }
        public virtual ICollection<TranslationUnitDefinition> TranslationUnitDefinitions { get; set; }
        public virtual ICollection<TranslationUnitSynonym> TranslationUnitSynonymSynonymTranslationUnits { get; set; }
        public virtual ICollection<TranslationUnitSynonym> TranslationUnitSynonymTranslationUnits { get; set; }
    }
}
