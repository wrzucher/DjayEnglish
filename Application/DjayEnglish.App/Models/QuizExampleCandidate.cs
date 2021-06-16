// -----------------------------------------------------------------------
// <copyright file="QuizExampleCandidate.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Administration.Models
{
    /// <summary>
    /// Object model which contain infromation about using and examples for candidate quiz.
    /// </summary>
    public partial class QuizExampleCandidate
    {
        /// <summary>
        /// Gets or sets id of word example.
        /// </summary>
        public int? TranslationUnitUsageId { get; set; }

        /// <summary>
        /// Gets or sets text of word usage.
        /// </summary>
        public string Text { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether usage will be include into quiz.
        /// </summary>
        public bool IsInclude { get; set; }
    }
}
