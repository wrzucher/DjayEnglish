// -----------------------------------------------------------------------
// <copyright file="Quize.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Object model which contain information about quize.
    /// </summary>
    public class Quize
    {
        /// <summary>
        /// Gets or sets id of quize.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of word definition which used in quize.
        /// </summary>
        public int WordDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether quize is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a date when quize was created.
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Gets or sets a date when quize was closed.
        /// </summary>
        public DateTimeOffset? Closed { get; set; }

        /// <summary>
        /// Gets or sets text of question.
        /// </summary>
        public string Question { get; set; } = null!;

        /// <summary>
        /// Gets or sets answer options for quize.
        /// </summary>
        public IEnumerable<QuizeAnswerOption> QuizeAnswerOptions { get; set; } = null!;

        /// <summary>
        /// Gets or sets using and examples for quize.
        /// </summary>
        public IEnumerable<QuizeExample> QuizeExamples { get; set; } = null!;
    }
}
