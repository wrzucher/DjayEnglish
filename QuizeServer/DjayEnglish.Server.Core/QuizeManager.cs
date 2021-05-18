// -----------------------------------------------------------------------
// <copyright file="QuizeManager.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core
{
    using System;
    using System.Collections.Generic;
    using DjayEnglish.Server.Core.EntityFrameworkCore;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Manager which work with quize.
    /// </summary>
    public class QuizeManager
    {
        private readonly QuizeManagerEvents quizeManagerEvents;
        private readonly DbQuizePersistence dbQuizePersistence;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizeManager"/> class.
        /// </summary>
        /// <param name="quizeManagerEvents">Global class with quize manager events.</param>
        /// <param name="dbQuizePersistence">Database context for quizzes representation.</param>
        public QuizeManager(
            QuizeManagerEvents quizeManagerEvents,
            DbQuizePersistence dbQuizePersistence)
        {
            this.quizeManagerEvents = quizeManagerEvents ?? throw new ArgumentNullException(nameof(quizeManagerEvents));
            this.dbQuizePersistence = dbQuizePersistence ?? throw new ArgumentNullException(nameof(dbQuizePersistence));
        }

        /// <summary>
        /// Start new quize for user.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <param name="startedAt">Time when new quize started.</param>
        /// <returns>New quize for user.</returns>
        public Quize StartQuize(long chatId, DateTimeOffset startedAt)
        {
            if (this.dbQuizePersistence.IsChatExist(chatId))
            {
                this.dbQuizePersistence.RegisterNewChat(chatId);
            }

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
        /// <param name="quizeId">Id of the requested quize.</param>
        /// <returns>Quize model if it found.</returns>
        public Quize? GetQuize(int quizeId)
        {
            return this.dbQuizePersistence.GetQuize(quizeId);
        }
    }
}
