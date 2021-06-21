// -----------------------------------------------------------------------
// <copyright file="QuizAnswerOptionCandidate.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain information about answer option for quiz candidate.
    /// </summary>
    public partial class QuizAnswerOptionCandidate
    {
        /// <summary>
        /// Gets or sets text of answer option.
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Gets or sets spelling text of source translation unit.
        /// </summary>
        public string? SourceTranslationUnit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether answer option is right.
        /// </summary>
        public bool IsRightAnswer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether answer option is include to quiz.
        /// </summary>
        public bool IsInclude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether translation unit antonym
        /// for translation unit used in quiz.
        /// </summary>
        public bool IsAntonym { get; set; }
    }
}
