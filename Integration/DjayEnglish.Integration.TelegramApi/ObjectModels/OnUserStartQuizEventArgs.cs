// -----------------------------------------------------------------------
// <copyright file="OnUserStartQuizEventArgs.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Integration.TelegramApi.ObjectModels
{
    /// <summary>
    /// Event args which contain information quiz start.
    /// </summary>
    public class OnUserStartQuizEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnUserStartQuizEventArgs"/> class.
        /// </summary>
        /// <param name="chatId">Id of the chat where quiz should be started.</param>
        public OnUserStartQuizEventArgs(
            long chatId)
        {
            this.ChatId = chatId;
        }

        /// <summary>
        /// Gets id of the chat where quiz should be started.
        /// </summary>
        public long ChatId { get; }
    }
}
