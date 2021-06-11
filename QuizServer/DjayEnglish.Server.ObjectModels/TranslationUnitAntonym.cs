// -----------------------------------------------------------------------
// <copyright file="TranslationUnitAntonym.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain information about translation unit antonym.
    /// </summary>
    public partial class TranslationUnitAntonym
    {
        /// <summary>
        /// Gets or sets id of antonym.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of translation unit.
        /// </summary>
        public int TranslationUnitId { get; set; }

        /// <summary>
        /// Gets or sets id of antonym's translation unit.
        /// </summary>
        public int AntonymTranslationUnitId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether synonym is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
