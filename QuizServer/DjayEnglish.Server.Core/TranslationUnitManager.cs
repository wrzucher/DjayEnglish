// -----------------------------------------------------------------------
// <copyright file="TranslationUnitManager.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core
{
    using System;
    using System.Collections.Generic;
    using DjayEnglish.Server.Core.EntityFrameworkCore;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Manager which work with translation units.
    /// </summary>
    public class TranslationUnitManager
    {
        private readonly DbTranslationUnitPersistence dbTranslationUnitPersistence;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationUnitManager"/> class.
        /// </summary>
        /// <param name="dbTranslationUnitPersistence">Database context for translation units representation.</param>
        public TranslationUnitManager(
            DbTranslationUnitPersistence dbTranslationUnitPersistence)
        {
            this.dbTranslationUnitPersistence = dbTranslationUnitPersistence ?? throw new ArgumentNullException(nameof(dbTranslationUnitPersistence));
        }

        /// <summary>
        /// Get translation units without definitions.
        /// </summary>
        /// <param name="languageType">Type of language.</param>
        /// <param name="partOfSpeech">Part of speach.</param>
        /// <param name="isActive">Indicate that translation unit is active.</param>
        /// <param name="withDefinitions">Indicate that translation unit should have definitions.</param>
        /// <param name="searchText">The part of search spelling of translation unit.</param>
        /// <param name="offset">Offset which will be used for skipping of returned collection.</param>
        /// <param name="page">Page number which will be used for returned collection.</param>
        /// <param name="pageSize">Page size which will be used for returned collection.</param>
        /// <returns>Translation units collection.</returns>
        public IEnumerable<ObjectModels.TranslationUnit> GetTranslationUnits(
            LanguageType languageType,
            PartOfSpeechType? partOfSpeech,
            bool isActive,
            bool? withDefinitions,
            string? searchText,
            int offset,
            int page,
            int pageSize)
        {
            var result = this.dbTranslationUnitPersistence.GetTranslationUnits(
                languageType,
                partOfSpeech,
                isActive,
                withDefinitions,
                searchText,
                offset,
                page,
                pageSize);
            return result;
        }
    }
}
