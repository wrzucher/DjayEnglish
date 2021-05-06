// -----------------------------------------------------------------------
// <copyright file="AnswerOption.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Core.ObjectModels
{
    /// <summary>
    /// Object model which contain information about answer option.
    /// </summary>
    public class AnswerOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AnswerOption"/> class.
        /// </summary>
        /// <param name="answer">Text of answer option.</param>
        /// <param name="isRight">Indicate that answer is right.</param>
        public AnswerOption(
            string answer,
            bool isRight)
        {
            this.Answer = answer;
            this.IsRight = isRight;
        }

        /// <summary>
        /// Gets text of answer option.
        /// </summary>
        public string Answer { get; }

        /// <summary>
        /// Gets a value indicating whether answer is right.
        /// </summary>
        public bool IsRight { get; }
    }
}
