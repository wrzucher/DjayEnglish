// -----------------------------------------------------------------------
// <copyright file="RegisterUserAnswerResponse.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Core.ObjectModels
{
    /// <summary>
    /// Object model which contain information about response on user answer.
    /// </summary>
    public class RegisterUserAnswerResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserAnswerResponse"/> class.
        /// </summary>
        /// <param name="isRight">Indicate that user answer is right.</param>
        public RegisterUserAnswerResponse(
            bool isRight)
        {
            this.IsRight = isRight;
        }

        /// <summary>
        /// Gets a value indicating whether user answer is right.
        /// </summary>
        public bool IsRight { get; }
    }
}
