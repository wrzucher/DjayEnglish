// -----------------------------------------------------------------------
// <copyright file="OnQuizeStartedEventArgs.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Core.ObjectModels
{
    /// <summary>
    /// Event args which contain information about started quize.
    /// </summary>
    public class OnQuizeStartedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnQuizeStartedEventArgs"/> class.
        /// </summary>
        /// <param name="chatId">Id of the chat where quize started.</param>
        /// <param name="quize">Started quize.</param>
        public OnQuizeStartedEventArgs(
            long chatId,
            Quize quize)
        {
            this.ChatId = chatId;
            this.Quize = quize;
        }

        /// <summary>
        /// Gets id of the chat where quize should be started.
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// Gets started quize.
        /// </summary>
        public Quize Quize { get; }
    }
}
