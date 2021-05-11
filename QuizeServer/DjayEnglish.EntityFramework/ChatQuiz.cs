using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class ChatQuiz
    {
        public int Id { get; set; }
        public int QuizeId { get; set; }
        public long ChatId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Closed { get; set; }
        public byte QuizeState { get; set; }

        public virtual Chat Chat { get; set; }
        public virtual Quiz Quize { get; set; }
    }
}
