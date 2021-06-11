// -----------------------------------------------------------------------
// <copyright file="TranslationUnit.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain information about word or phrase.
    /// </summary>
    public partial class TranslationUnit
    {
        /// <summary>
        /// Gets or sets id of translation unit.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of word on which definition made.
        /// </summary>
        public string Spelling { get; set; } = null!;

        /// <summary>
        /// Gets or sets part of speech type.
        /// </summary>
        public PartOfSpeechType PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets language type.
        /// </summary>
        public LanguageType Language { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether translation unit is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets definitions for translation unit.
        /// </summary>
        public IEnumerable<TranslationUnitDefinition> Definitions { get; set; } = null!;

        /// <summary>
        /// Gets or sets synonyms for translation unit.
        /// </summary>
        public IEnumerable<TranslationUnitSynonym> Sysnonyms { get; set; } = null!;

        /// <summary>
        /// Gets or sets antonyms for translation unit.
        /// </summary>
        public IEnumerable<TranslationUnitAntonym> Antonyms { get; set; } = null!;
    }
}
