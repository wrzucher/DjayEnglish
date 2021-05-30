// -----------------------------------------------------------------------
// <copyright file="TranslationUnitUsage.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Object model which contain infromation about translation unit usage.
    /// </summary>
    public partial class TranslationUnitUsage
    {
        /// <summary>
        /// Gets or sets id of translation unit example.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of translation unit definition.
        /// </summary>
        public int TranslationUnitDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets text of translation unit example.
        /// </summary>
        public string Example { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether usage is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
