using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class User
    {
        public User()
        {
            UserQuizAnswers = new HashSet<UserQuizAnswer>();
            UserQuizzes = new HashSet<UserQuiz>();
        }

        public string Id { get; set; }
        public long? ChatId { get; set; }

        public virtual ICollection<UserQuizAnswer> UserQuizAnswers { get; set; }
        public virtual ICollection<UserQuiz> UserQuizzes { get; set; }
    }
}
