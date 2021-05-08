using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class WordAntonym
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public int AntonymWordId { get; set; }
        public bool IsActive { get; set; }

        public virtual Word Word { get; set; }
    }
}
