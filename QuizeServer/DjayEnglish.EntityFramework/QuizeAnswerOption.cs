using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class QuizeAnswerOption
    {
        public int Id { get; set; }
        public int QuizeId { get; set; }
        public string ShowedKey { get; set; }
        public string Text { get; set; }
        public bool IsRightAnswer { get; set; }

        public virtual Quiz Quize { get; set; }
    }
}
