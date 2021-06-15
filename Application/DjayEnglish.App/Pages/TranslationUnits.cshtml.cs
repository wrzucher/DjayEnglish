// -----------------------------------------------------------------------
// <copyright file="TranslationUnits.cshtml.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Administration.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using DjayEnglish.Administration.Models;
    using DjayEnglish.Server.Core;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Translation units page model.
    /// </summary>
    public partial class TranslationUnitsModel : PageModel
    {
        private const int DefaultPageSize = 10;
        private readonly TranslationUnitManager translationUnitManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslationUnitsModel"/> class.
        /// </summary>
        /// <param name="translationUnitManager">Quiz manager for use.</param>
        /// <param name="mapper">Object mapper for use.</param>
        public TranslationUnitsModel(
            TranslationUnitManager translationUnitManager,
            IMapper mapper)
        {
            this.translationUnitManager = translationUnitManager ?? throw new ArgumentNullException(nameof(translationUnitManager));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets or sets offset for returned collection.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int? Offset { get; set; }

        /// <summary>
        /// Gets or sets number of page for returned collection.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets page size for returned collection.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether translation unit is active.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets or sets language type.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public Server.ObjectModels.LanguageType? Language { get; set; }

        /// <summary>
        /// Gets or sets part of speech of translation units.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public Server.ObjectModels.PartOfSpeechType? PartOfSpeech { get; set; }

        /// <summary>
        /// Gets or sets indicate that translation unit has to have definition.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public bool? WithDefinition { get; set; }

        /// <summary>
        /// Gets or sets text which will be used for searching by translation unit spelling.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string? SearchText { get; set; }

        /// <summary>
        /// Gets quizzes.
        /// </summary>
        public IEnumerable<TranslationUnit> TranslationUnits { get; private set; } = Array.Empty<TranslationUnit>();

        /// <summary>
        /// Displays list of the games.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task OnGet()
        {
            var offsetEstimated = this.Offset ?? 0;
            var pageEstimated = this.PageNumber ?? 1;
            var pageSizeEstimated = this.PageSize ?? DefaultPageSize;

            var translationUnits = this.translationUnitManager.GetTranslationUnits(
                this.Language ?? Server.ObjectModels.LanguageType.AmericanEnglish,
                this.PartOfSpeech,
                this.IsActive ?? false,
                this.WithDefinition,
                this.SearchText,
                offsetEstimated,
                pageEstimated,
                pageSizeEstimated);
            var translationUnitsModels = this.mapper.Map<IEnumerable<TranslationUnit>>(translationUnits);

            this.TranslationUnits = translationUnitsModels;
            return Task.CompletedTask;
        }
    }
}
