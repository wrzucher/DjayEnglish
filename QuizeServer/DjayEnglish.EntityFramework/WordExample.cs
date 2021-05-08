using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class WordExample
    {
        public WordExample()
        {
            QuizeExamples = new HashSet<QuizeExample>();
        }

        public int Id { get; set; }
        public int WordDefinitionId { get; set; }
        public int Examle { get; set; }
        public bool IsActive { get; set; }

        public virtual WordDefinition WordDefinition { get; set; }
        public virtual ICollection<QuizeExample> QuizeExamples { get; set; }
    }
}
