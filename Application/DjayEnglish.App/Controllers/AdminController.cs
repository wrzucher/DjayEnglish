// -----------------------------------------------------------------------
// <copyright file="AdminController.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Home controller.
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// Index view.
        /// </summary>
        /// <returns>ActionResult which contain View.</returns>
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
