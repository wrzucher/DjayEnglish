// -----------------------------------------------------------------------
// <copyright file="QuizExample.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Administration.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain infromation about using and examples for quiz.
    /// </summary>
    public partial class QuizExample
    {
        /// <summary>
        /// Gets or sets id of quiz example.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of quiz to which example related.
        /// </summary>
        public int QuizId { get; set; }

        /// <summary>
        /// Gets or sets id of word example.
        /// </summary>
        public int TranslationUnitUsageId { get; set; }

        /// <summary>
        /// Gets or sets model of word usage.
        /// </summary>
        public TranslationUnitUsage TranslationUnitUsage { get; set; } = null!;
    }
}
