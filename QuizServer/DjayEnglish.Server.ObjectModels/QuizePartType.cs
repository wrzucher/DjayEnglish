// -----------------------------------------------------------------------
// <copyright file="QuizePartType.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Descibe quize part type.
    /// </summary>
    public enum QuizePartType
    {
        /// <summary>
        /// The qize part which contain question.
        /// </summary>
        Question = 0,

        /// <summary>
        /// The qize part which contain answer options on question.
        /// </summary>
        AnswerOptions = 1,

        /// <summary>
        /// The qize part which contain word usage.
        /// </summary>
        Usage = 2,
    }
}
