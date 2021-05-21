using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class QuizeExample
    {
        public int Id { get; set; }
        public int QuizeId { get; set; }
        public int WordUsageId { get; set; }

        public virtual Quiz Quize { get; set; }
        public virtual WordUsage WordUsage { get; set; }
    }
}
