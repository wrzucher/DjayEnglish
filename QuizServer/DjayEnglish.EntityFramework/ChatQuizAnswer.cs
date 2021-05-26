using System;
using System.Collections.Generic;

#nullable disable

namespace DjayEnglish.EntityFramework
{
    public partial class ChatQuizAnswer
    {
        public int Id { get; set; }
        public int ChatQuizId { get; set; }
        public int? AnswerId { get; set; }

        public virtual QuizAnswerOption Answer { get; set; }
    }
}
