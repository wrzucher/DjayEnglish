// -----------------------------------------------------------------------
// <copyright file="TranslationUnitSynonym.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain information about translation unit synonyms.
    /// </summary>
    public partial class TranslationUnitSynonym
    {
        /// <summary>
        /// Gets or sets id of synonym.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of translation unit.
        /// </summary>
        public int TranslationUnitId { get; set; }

        /// <summary>
        /// Gets or sets id of synonym's translation unit.
        /// </summary>
        public int SynonymTranslationUnitId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether synonym is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
