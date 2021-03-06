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
            var tuEntity = this.dbContext.TranslationUnits
                .Include(_ => _.TranslationUnitDefinitions)
                .Where(_ =>
                    _.IsActive == isActive
                 && _.Language == (byte)languageType);
            if (withDefinitions != null)
            {
                if (withDefinitions.Value)
                {
                    tuEntity = tuEntity.Where(_ => _.TranslationUnitDefinitions.Any());
                }
                else
                {
                    tuEntity = tuEntity.Where(_ => !_.TranslationUnitDefinitions.Any());
                }
            }

            if (partOfSpeech != null)
            {
                tuEntity = tuEntity.Where(_ => _.PartOfSpeech == (byte)partOfSpeech.Value);
            }

            if (searchText != null)
            {
                tuEntity = tuEntity.Where(_ => _.Spelling.Contains(searchText));
            }

            tuEntity = tuEntity.Page(offset, page, pageSize);
            var tuModel = this.mapper.Map<IEnumerable<ObjectModels.TranslationUnit>>(tuEntity);
            return tuModel;
        }

        /// <summary>
        /// Get translation units with definitions.
        /// </summary>
        /// <param name="languageType">Type of language.</param>
        /// <param name="partOfSpeech">Part of speach.</param>
        /// <param name="isActive">Indicate that translation unit is active.</param>
        /// <param name="definitionsRequired">Indicate that translation unit should have definitions.</param>
        /// <param name="searchSpelling">The part of search spelling of translation unit.</param>
        /// <returns>Translation units collection.</returns>
        public IEnumerable<ObjectModels.TranslationUnit> GetTranslationUnits(
            LanguageType languageType,
            PartOfSpeechType partOfSpeech,
            bool isActive,
            bool definitionsRequired,
            string? searchSpelling)
        {
            var tuEntity = this.dbContext.TranslationUnits
                .Include(_ => _.TranslationUnitDefinitions)
                .Where(_ =>
                    _.IsActive == isActive
                 && _.Language == (byte)languageType
                 && _.PartOfSpeech == (byte)partOfSpeech);
            if (definitionsRequired)
            {
                tuEntity = tuEntity.Where(_ => _.TranslationUnitDefinitions.Any());
            }

            if (searchSpelling != null)
            {
                tuEntity = tuEntity.Where(_ => _.Spelling.StartsWith(searchSpelling));
            }

            var tuModel = this.mapper.Map<IEnumerable<ObjectModels.TranslationUnit>>(tuEntity);
            return tuModel;
        }

        /// <summary>
        /// Get translation unit by id.
        /// </summary>
        /// <param name="translationUnitId">Id of translation unit.</param>
        /// <returns>Translation unit model.</returns>
        public ObjectModels.TranslationUnit? GetTranslationUnit(int translationUnitId)
        {
            var tuEntity = this.dbContext.TranslationUnits
                .Include(_ => _.TranslationUnitDefinitions)
                .Include(_ => _.TranslationUnitSynonymTranslationUnits)
                .Include(_ => _.TranslationUnitAntonymTranslationUnits)
                .FirstOrDefault(_ => _.Id == translationUnitId);
            var tuModel = this.mapper.Map<ObjectModels.TranslationUnit?>(tuEntity);
            return tuModel;
        }

        /// <summary>
        /// Get translation unit definition by id.
        /// </summary>
        /// <param name="translationUnitDefinitionId">Id of translation unit definition.</param>
        /// <returns>Translation unit definition model.</returns>
        public ObjectModels.TranslationUnitDefinition? GetTranslationUnitDefinition(
            int translationUnitDefinitionId)
        {
            var tuEntity = this.dbContext.TranslationUnitDefinitions
                .Include(_ => _.TranslationUnit)
                .Include(_ => _.TranslationUnitUsages)
                .FirstOrDefault(_ => _.Id == translationUnitDefinitionId);
            var tuModel = this.mapper.Map<ObjectModels.TranslationUnitDefinition?>(tuEntity);
            return tuModel;
        }

        /// <summary>
        /// Get translation units by pattern.
        /// </summary>
        /// <param name="languageType">Type of language.</param>
        /// <param name="partOfSpeech">Part of speach.</param>
        /// <param name="exclusiveSpelling">Spelling of translation units which will be exclude from query.</param>
        /// <param name="unitsAmount">Amount of returned translation units.</param>
        /// <param name="maxDefinitionsLength">Max definition length for translation units.</param>
        /// <param name="spellingStartWith">Pattern which will be used for search.</param>
        /// <returns>Translation units collection.</returns>
        public IEnumerable<ObjectModels.TranslationUnit> GetTranslationUnits(
            LanguageType languageType,
            PartOfSpeechType partOfSpeech,
            string[] exclusiveSpelling,
            int unitsAmount,
            int maxDefinitionsLength,
            string? spellingStartWith)
        {
            var tuEntity = this.dbContext.TranslationUnits
                .Include(_ => _.TranslationUnitDefinitions)
                .Where(_ =>
                    !exclusiveSpelling.Contains(_.Spelling)
                 && _.Language == (byte)languageType
                 && _.PartOfSpeech == (byte)partOfSpeech
                 && _.TranslationUnitDefinitions
                    .Where(td => td.Definition.Length < maxDefinitionsLength)
                    .Any());

            if (spellingStartWith != null)
            {
                tuEntity = tuEntity.Where(_ => _.Spelling.StartsWith(spellingStartWith));
            }

            tuEntity = tuEntity.Take(unitsAmount);

            var tuModel = this.mapper.Map<IEnumerable<ObjectModels.TranslationUnit>>(tuEntity);
            return tuModel;
        }

        /// <summary>
        /// Get translation unit usage.
        /// </summary>
        /// <param name="translationUnitDefinitionId">Id of translation units for which return usage.</param>
        /// <param name="isActive">Indicate that return only active usage.</param>
        /// <returns>Translation unit usage collection.</returns>
        public IEnumerable<ObjectModels.TranslationUnitUsage> GetTranslationUnitUsage(
            int translationUnitDefinitionId,
            bool isActive)
        {
            var tuEntity = this.dbContext.TranslationUnitUsages
                .Where(_ =>
                _.IsActive == isActive
             && _.TranslationUnitDefinitionId == translationUnitDefinitionId);
            var tuModel = this.mapper.Map<IEnumerable<ObjectModels.TranslationUnitUsage>>(tuEntity);
            return tuModel;
        }

        /// <summary>
        /// Get translation units by Ids.
        /// </summary>
        /// <param name="translationUnitIds">Id of translation units which will be returned.</param>
        /// <returns>Translation units collection.</returns>
        public IEnumerable<ObjectModels.TranslationUnit> GetTranslationUnits(
            int[] translationUnitIds)
        {
            var tuEntity = this.dbContext.TranslationUnits
                .Include(_ => _.TranslationUnitDefinitions)
                .Include(_ => _.TranslationUnitSynonymTranslationUnits)
                .Include(_ => _.TranslationUnitAntonymTranslationUnits)
                .Where(_ => translationUnitIds.Contains(_.Id));
            var tuModel = this.mapper.Map<IEnumerable<ObjectModels.TranslationUnit>>(tuEntity);
            return tuModel;
        }
    }
}
