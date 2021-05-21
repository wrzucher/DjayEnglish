// -----------------------------------------------------------------------
// <copyright file="QuizeAnswerOption.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain information about answer option for quize.
    /// </summary>
    public partial class QuizeAnswerOption
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuizeAnswerOption"/> class.
        /// </summary>
        /// <param name="id">Id of answer option.</param>
        /// <param name="quizeId">Id of quize to which answer option related.</param>
        /// <param name="showedKey">Showed to user key.</param>
        /// <param name="text">Text of answer options.</param>
        /// <param name="isRightAnswer">Indicate that answer options is right.</param>
        public QuizeAnswerOption(
            int id,
            int quizeId,
            string showedKey,
            string text,
            bool isRightAnswer)
        {
            this.Id = id;
            this.QuizeId = quizeId;
            this.ShowedKey = showedKey;
            this.Text = text;
            this.IsRightAnswer = isRightAnswer;
        }

        /// <summary>
        /// Gets id of the answer option.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets id of quize to which answer option related.
        /// </summary>
        public int QuizeId { get; }

        /// <summary>
        /// Gets showed to user key.
        /// </summary>
        public string ShowedKey { get; }

        /// <summary>
        /// Gets text of answer option.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets a value indicating whether answer option is right.
        /// </summary>
        public bool IsRightAnswer { get; }
    }
}
