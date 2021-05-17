// -----------------------------------------------------------------------
// <copyright file="ChatQuizeState.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Descibe states in which chat quize could be.
    /// </summary>
    public enum ChatQuizeState
    {
        /// <summary>
        /// The user is studying this quize at this moment.
        /// </summary>
        IsActive = 0,

        /// <summary>
        /// The user is finished studying this quize.
        /// </summary>
        IsCompleted = 1,

        /// <summary>
        /// The quize was terminated for this chat.
        /// </summary>
        IsTerminate = 255,
    }
}
