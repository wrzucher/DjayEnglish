// -----------------------------------------------------------------------
// <copyright file="WordUsage.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Object model which contain infromation about word usage.
    /// </summary>
    public partial class WordUsage
    {
        /// <summary>
        /// Gets or sets id of word example.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of word definition.
        /// </summary>
        public int WordDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets text of word example.
        /// </summary>
        public string Example { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether usage is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
