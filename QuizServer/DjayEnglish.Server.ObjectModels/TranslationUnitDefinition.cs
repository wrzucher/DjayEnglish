// -----------------------------------------------------------------------
// <copyright file="TranslationUnitDefinition.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Object model which contain information about word or phrase definition.
    /// </summary>
    public partial class TranslationUnitDefinition
    {
        /// <summary>
        /// Gets or sets id of translation unit definition.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of translation unit on which definition made.
        /// </summary>
        public int TranslationUnitId { get; set; }

        /// <summary>
        /// Gets or sets word definition.
        /// </summary>
        public string Definition { get; set; } = null!;

        /// <summary>
        /// Gets or sets id of definition source.
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        /// Gets or sets name of definition source.
        /// </summary>
        public string SourceName { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether definition is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
