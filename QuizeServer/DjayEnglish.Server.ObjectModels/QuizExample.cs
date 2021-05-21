// -----------------------------------------------------------------------
// <copyright file="QuizExample.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
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
        public int QuizeId { get; set; }

        /// <summary>
        /// Gets or sets id of word example.
        /// </summary>
        public int WordExampleId { get; set; }

        /// <summary>
        /// Gets or sets model of word usage.
        /// </summary>
        public WordUsage WordUsage { get; set; } = null!;
    }
}
