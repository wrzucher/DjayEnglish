using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class UserQuizAnswer
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? AnswerId { get; set; }

        public virtual QuizAnswerOption Answer { get; set; }
        public virtual User User { get; set; }
    }
}
