// -----------------------------------------------------------------------
// <copyright file="Quizzes.cshtml.cs" company="DjayEnglish">
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
    /// Quizzes page model.
    /// </summary>
    internal partial class QuizzesModel : PageModel
    {
        private readonly QuizeManager quizeManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizzesModel"/> class.
        /// </summary>
        /// <param name="quizeManager">Quize manager for use.</param>
        /// <param name="mapper">Object mapper for use.</param>
        public QuizzesModel(
            QuizeManager quizeManager,
            IMapper mapper)
        {
            this.quizeManager = quizeManager ?? throw new ArgumentNullException(nameof(quizeManager));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets or sets date from which display data.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public DateTimeOffset? FromDate { get; set; }

        /// <summary>
        /// Gets or sets date up to which display data.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public DateTimeOffset? ToDate { get; set; }

        /// <summary>
        /// Gets quizzes.
        /// </summary>
        internal IEnumerable<Quize> Quizzes { get; private set; } = Array.Empty<Quize>();

        /// <summary>
        /// Displays list of the games.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task OnGet()
        {
            var quizzes = this.quizeManager.GetQuizzes();
            var quizeModels = this.mapper.Map<IEnumerable<Quize>>(quizzes);
            this.Quizzes = quizeModels;
            return Task.CompletedTask;
        }
    }
}
