using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class UserQuizAnswer
    {
        public int Id { get; set; }
        public int UserQuizeId { get; set; }
        public int? AnswerId { get; set; }

        public virtual QuizAnswerOption Answer { get; set; }
        public virtual UserQuiz UserQuize { get; set; }
    }
}
