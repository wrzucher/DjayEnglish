// -----------------------------------------------------------------------
// <copyright file="UserQuiz.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    using System;

    /// <summary>
    /// Object model which contain information about user's quiz.
    /// </summary>
    public partial class UserQuiz
    {
        /// <summary>
        /// Gets or sets user quiz id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets quiz id.
        /// </summary>
        public int QuizId { get; set; }

        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets date when quiz was started for user.
        /// </summary>
        public DateTimeOffset Started { get; set; }

        /// <summary>
        /// Gets or sets date when quiz was ended for user.
        /// </summary>
        public DateTimeOffset? Ended { get; set; }

        /// <summary>
        /// Gets or sets quiz state.
        /// </summary>
        public QuizState State { get; set; }
    }
}
