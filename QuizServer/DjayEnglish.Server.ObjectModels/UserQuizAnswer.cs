// -----------------------------------------------------------------------
// <copyright file="UserQuizAnswer.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Object model which contain information about user's quiz answer.
    /// </summary>
    public partial class UserQuizAnswer
    {
        /// <summary>
        /// Gets or sets id of user's quiz answer.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of user.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// Gets or sets id of quiz answer option.
        /// </summary>
        public int? AnswerId { get; set; }
    }
}
