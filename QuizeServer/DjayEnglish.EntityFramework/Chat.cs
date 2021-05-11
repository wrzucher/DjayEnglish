using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class Chat
    {
        public Chat()
        {
            ChatQuizzes = new HashSet<ChatQuiz>();
        }

        public long Id { get; set; }

        public virtual ICollection<ChatQuiz> ChatQuizzes { get; set; }
    }
}
