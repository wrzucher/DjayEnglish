// -----------------------------------------------------------------------
// <copyright file="NewQuiz.cshtml.cs" company="DjayEnglish">
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
    using DjayEnglish.Server.ObjectModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using QuizCandidate = DjayEnglish.Administration.Models.QuizCandidate;

    /// <summary>
    /// Create new quiz page model.
    /// </summary>
    public partial class NewQuizModel : PageModel
    {
        private readonly QuizManager quizManager;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewQuizModel"/> class.
        /// </summary>
        /// <param name="quizManager">Quiz manager for use.</param>
        /// <param name="mapper">Object mapper for use.</param>
        public NewQuizModel(
            QuizManager quizManager,
            IMapper mapper)
        {
            this.quizManager = quizManager ?? throw new ArgumentNullException(nameof(quizManager));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Gets or sets quiz candidate related to translation unit definition.
        /// </summary>
        public QuizCandidate QuizCandidate { get; set; } = null!;

        /// <summary>
        /// Gets or sets id of translation unit definition related to quiz.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int TranslationUnitDefinitionId { get; set; }

        /// <summary>
        /// Displays quiz candidate.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task OnGet()
        {
            var quizCandidate = this.quizManager.GenerateQuizCandidate(
                this.TranslationUnitDefinitionId,
                10,
                150,
                3,
                3,
                DateTimeOffset.UtcNow);
            this.QuizCandidate = this.mapper.Map<QuizCandidate>(quizCandidate);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Create quiz from quiz candidate.
        /// </summary>
        /// <param name="quizCandidate">I am steel thinking about this param.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task OnPost(QuizCandidate quizCandidate)
        {
            return Task.CompletedTask;
        }
    }
}
