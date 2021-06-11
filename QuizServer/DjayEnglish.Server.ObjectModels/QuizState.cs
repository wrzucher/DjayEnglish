// -----------------------------------------------------------------------
// <copyright file="QuizState.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Descibe states in which quiz could be for user.
    /// </summary>
    public enum QuizState
    {
        /// <summary>
        /// The user is studying this quiz at this moment.
        /// </summary>
        IsActive = 0,

        /// <summary>
        /// The user is finished studying this quiz.
        /// </summary>
        IsCompleted = 1,

        /// <summary>
        /// The quiz was terminated for this chat.
        /// </summary>
        IsTerminate = 255,
    }
}
