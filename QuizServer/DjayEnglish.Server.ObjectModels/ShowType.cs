// -----------------------------------------------------------------------
// <copyright file="ShowType.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Descibe type as question, answers or examples could be showed to user.
    /// </summary>
    public enum ShowType
    {
        /// <summary>
        /// The user sees only text.
        /// </summary>
        Text = 0,

        /// <summary>
        /// The user sees only audio.
        /// </summary>
        Audio = 1,

        /// <summary>
        /// The user does not see anything.
        /// </summary>
        None = 255,
    }
}
