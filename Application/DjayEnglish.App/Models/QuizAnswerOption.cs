// -----------------------------------------------------------------------
// <copyright file="QuizAnswerOption.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Administration.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain information about answer option for quiz.
    /// </summary>
    public partial class QuizAnswerOption
    {
        /// <summary>
        /// Gets or sets id of the answer option.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of quiz to which answer option related.
        /// </summary>
        public int QuizeId { get; set; }

        /// <summary>
        /// Gets or sets text of answer option.
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether answer option is right.
        /// </summary>
        public bool IsRightAnswer { get; set; }
    }
}
