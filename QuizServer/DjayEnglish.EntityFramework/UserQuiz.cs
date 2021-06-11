using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class UserQuiz
    {
        public UserQuiz()
        {
            UserQuizAnswers = new HashSet<UserQuizAnswer>();
        }

        public int Id { get; set; }
        public int QuizId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset Started { get; set; }
        public DateTimeOffset? Ended { get; set; }
        public byte State { get; set; }

        public virtual Quiz Quiz { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<UserQuizAnswer> UserQuizAnswers { get; set; }
    }
}
