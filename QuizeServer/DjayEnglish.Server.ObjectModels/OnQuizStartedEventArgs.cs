// -----------------------------------------------------------------------
// <copyright file="OnQuizStartedEventArgs.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Event args which contain information about started quiz.
    /// </summary>
    public class OnQuizStartedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnQuizStartedEventArgs"/> class.
        /// </summary>
        /// <param name="chatId">Id of the chat where quiz started.</param>
        /// <param name="quiz">Started quiz.</param>
        public OnQuizStartedEventArgs(
            long chatId,
            Quiz quiz)
        {
            this.ChatId = chatId;
            this.Quiz = quiz;
        }

        /// <summary>
        /// Gets id of the chat where quiz should be started.
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// Gets started quiz.
        /// </summary>
        public Quiz Quiz { get; }
    }
}
