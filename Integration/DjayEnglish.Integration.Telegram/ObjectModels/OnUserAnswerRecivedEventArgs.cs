// -----------------------------------------------------------------------
// <copyright file="OnUserAnswerRecivedEventArgs.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Integration.Telegram.ObjectModels
{
    /// <summary>
    /// Event args which contain information about recived user answer.
    /// </summary>
    public class OnUserAnswerRecivedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OnUserAnswerRecivedEventArgs"/> class.
        /// </summary>
        /// <param name="chatId">Id of the chat where quize should be started.</param>
        /// <param name="answer">Text of recived user answer.</param>
        public OnUserAnswerRecivedEventArgs(
            long chatId,
            string answer)
        {
            this.ChatId = chatId;
            this.Answer = answer;
        }

        /// <summary>
        /// Gets id of the chat where quize should be started.
        /// </summary>
        public long ChatId { get; }

        /// <summary>
        /// Gets text of recived user answer.
        /// </summary>
        public string Answer { get; }
    }
}
