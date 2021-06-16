// -----------------------------------------------------------------------
// <copyright file="QuizAnswerOptionCandidate.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Administration.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain information about answer option for candidate quiz.
    /// </summary>
    public partial class QuizAnswerOptionCandidate
    {
        /// <summary>
        /// Gets or sets text of answer option.
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether answer option is right.
        /// </summary>
        public bool IsRightAnswer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether answer option will be include into quiz.
        /// </summary>
        public bool IsInclude { get; set; }

        /// <summary>
        /// Gets or sets spelling text of source translation unit.
        /// </summary>
        public string? SourceTranslationUnit { get; set; }
    }
}
