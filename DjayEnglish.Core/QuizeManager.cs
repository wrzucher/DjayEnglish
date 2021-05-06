﻿// -----------------------------------------------------------------------
// <copyright file="QuizeManager.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Core
{
    using System;
    using System.Collections.Generic;
    using DjayEnglish.Core.ObjectModels;

    /// <summary>
    /// Manager which work with quize.
    /// </summary>
    public class QuizeManager
    {
        private readonly QuizeManagerEvents quizeManagerEvents;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizeManager"/> class.
        /// </summary>
        /// <param name="quizeManagerEvents">Global class with quize manager events.</param>
        public QuizeManager(
            QuizeManagerEvents quizeManagerEvents)
        {
            this.quizeManagerEvents = quizeManagerEvents ?? throw new ArgumentNullException(nameof(quizeManagerEvents));
        }

        /// <summary>
        /// Start new quize for user.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <returns>New quize for user.</returns>
        public Quize StartQuize(long chatId)
        {
            var startedQuize = this.GetQuize(1) ?? throw new InvalidOperationException();
            this.quizeManagerEvents.NotifyQuizeStarted(chatId, startedQuize);
            return startedQuize;
        }

        /// <summary>
        /// Register user answer.
        /// </summary>
        /// <param name="chatId">Chat Id.</param>
        /// <param name="userAnswerkey">User answer key.</param>
        public void RegisterUserAnswer(long chatId, string userAnswerkey)
        {
            var quize = this.GetQuize(1) ?? throw new InvalidOperationException();
            var isRightAnswer = quize.IsRightAnswer(userAnswerkey);
            this.quizeManagerEvents.NotifyUserAnswerResultRecived(chatId, isRightAnswer);
        }

        /// <summary>
        /// Get quize by id.
        /// </summary>
        /// <param name="quizeId">Id of the quize.</param>
        /// <returns>Quize if it found.</returns>
        public Quize? GetQuize(int quizeId)
        {
            var quiestion = "What does 'admire' mean?";
            var exemples = new string[]
            {
                "I want a guy I can look up to and admire.",
                "Most of all, they will admire.",
                "I cannot help but admire her dignity.",
            };
            var answerOptions = new Dictionary<string, AnswerOption>();
            answerOptions.Add("1", new AnswerOption("To find someone or something attractive and pleasant to look at.", true));
            answerOptions.Add("2", new AnswerOption("To dislike someone or something very much.", false));
            answerOptions.Add("3", new AnswerOption("With little or no light.", false));
            answerOptions.Add("4", new AnswerOption("Nearer to black than white in colour.", false));

            return new Quize(quizeId, quiestion, exemples, answerOptions);
        }
    }
}