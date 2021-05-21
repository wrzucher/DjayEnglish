// -----------------------------------------------------------------------
// <copyright file="QuizeExample.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Administration.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain infromation about using and examples for quize.
    /// </summary>
    public partial class QuizeExample
    {
        /// <summary>
        /// Gets or sets id of quize example.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of quize to which example related.
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
