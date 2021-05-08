// -----------------------------------------------------------------------
// <copyright file="OnUserAnswerResultEventArgs.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Object model which contain information about response on user answer.
    /// </summary>
    public class OnUserAnswerResultEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnUserAnswerResultEventArgs"/> class.
        /// </summary>
        /// <param name="chatId">Id of the chat where user answer on quize.</param>
        /// <param name="isAnswerRight">Indicate that user answer is right.</param>
        public OnUserAnswerResultEventArgs(
            long chatId,
            bool isAnswerRight)
        {
            this.ChatId = chatId;
            this.IsAnswerRight = isAnswerRight;
        }

        /// <summary>
        /// Gets id of the chat where user answer on quize.
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// Gets a value indicating whether user answer is right.
        /// </summary>
        public bool IsAnswerRight { get; }
    }
}
