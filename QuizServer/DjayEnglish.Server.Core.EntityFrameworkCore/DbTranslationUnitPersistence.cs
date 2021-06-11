// -----------------------------------------------------------------------
// <copyright file="DbTranslationUnitPersistence.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DjayEnglish.EntityFramework;
    using DjayEnglish.Server.ObjectModels;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Translation unit persistence.
    /// </summary>
    public class DbTranslationUnitPersistence
    {
        private readonly DjayEnglishDBContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbTranslationUnitPersistence"/> class.
        /// </summary>
        /// <param name="dbContext">Database context to use.</param>
        /// <param name="mapper">Map entity models to object models.</param>
        public DbTranslationUnitPersistence(
            DjayEnglishDBContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get translation units without definitions.
        /// </summary>
        /// <param name="languageType">Type of language.</param>
        /// <param name="partOfSpeech">Part of speach.</param>
        /// <param name="isActive">Indicate that translation unit is active.</param>
        /// <param name="searchSpelling">The part of search spelling of translation unit.</param>
        /// <returns>Translation units collection.</returns>
        public IEnumerable<ObjectModels.TranslationUnit> GetTranslationUnits(
            LanguageType languageType,
            PartOfSpeechType partOfSpeech,
            bool isActive,
            string? searchSpelling)
        {
            var tuEntity = this.dbContext.TranslationUnits
                .Where(_ =>
                    _.IsActive == isActive
                 && _.Language == (byte)languageType
                 && _.PartOfSpeech == (byte)partOfSpeech);
            if (searchSpelling != null)
            {
                tuEntity = tuEntity.Where(_ => _.Spelling.Contains(searchSpelling));
            }

            var tuModel = this.mapper.Map<IEnumerable<ObjectModels.TranslationUnit>>(tuEntity);
            return tuModel;
        }
    }
}
