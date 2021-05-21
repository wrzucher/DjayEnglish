using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class ChatQuiz
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public long ChatId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Closed { get; set; }
        public byte State { get; set; }

        public virtual Chat Chat { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
