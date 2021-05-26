using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class Word
    {
        public Word()
        {
            WordAntonyms = new HashSet<WordAntonym>();
            WordDefinitions = new HashSet<WordDefinition>();
            WordSynonymSynonymWords = new HashSet<WordSynonym>();
            WordSynonymWords = new HashSet<WordSynonym>();
        }

        public int Id { get; set; }
        public string Word1 { get; set; }
        public byte PartOfSpeech { get; set; }
        public byte Language { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<WordAntonym> WordAntonyms { get; set; }
        public virtual ICollection<WordDefinition> WordDefinitions { get; set; }
        public virtual ICollection<WordSynonym> WordSynonymSynonymWords { get; set; }
        public virtual ICollection<WordSynonym> WordSynonymWords { get; set; }
    }
}
