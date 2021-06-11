using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class QuizAnswerOption
    {
        public QuizAnswerOption()
        {
            UserQuizAnswers = new HashSet<UserQuizAnswer>();
        }

        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Text { get; set; }
        public bool IsRightAnswer { get; set; }

        public virtual Quiz Quiz { get; set; }
        public virtual ICollection<UserQuizAnswer> UserQuizAnswers { get; set; }
    }
}
