// -----------------------------------------------------------------------
// <copyright file="Quizzes.cshtml.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace LiveSoccer.Administration.Pages
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Quizzes page model.
    /// </summary>
    internal partial class QuizzesModel : PageModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuizzesModel"/> class.
        /// </summary>
        public QuizzesModel()
        {
        }

        /// <summary>
        /// Displays list of the games.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task OnGet()
        {
            return Task.CompletedTask;
        }
    }
}
