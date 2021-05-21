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
        private readonly QuizManager quizManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizzesModel"/> class.
        /// </summary>
        /// <param name="quizManager">Quiz manager for use.</param>
        /// <param name="mapper">Object mapper for use.</param>
        public QuizzesModel(
            QuizManager quizManager,
            IMapper mapper)
        {
            this.quizManager = quizManager ?? throw new ArgumentNullException(nameof(quizManager));
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
        /// Gets or sets a value indicating whether quiz is active.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public bool? IsActive { get; set; }

        /// <summary>
        /// Gets quizzes.
        /// </summary>
        internal IEnumerable<Quiz> Quizzes { get; private set; } = Array.Empty<Quiz>();

        /// <summary>
        /// Displays list of the games.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task OnGet()
        {
            var fromDate = this.FromDate ?? DateTimeOffset.UtcNow.AddDays(-1);
            var toDate = this.ToDate ?? DateTimeOffset.UtcNow;
            var quizzes = this.quizManager.GetQuizzes(
                fromDate,
                toDate,
                this.IsActive);
            var quizModels = this.mapper.Map<IEnumerable<Quiz>>(quizzes);
            this.Quizzes = quizModels;
            return Task.CompletedTask;
        }
    }
}
