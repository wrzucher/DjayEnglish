﻿// -----------------------------------------------------------------------
// <copyright file="QuizManager.cs" company="DjayEnglish">
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
    /// Manager which work with quiz.
    /// </summary>
    public class QuizManager
    {
        private readonly QuizManagerEvents quizManagerEvents;
        private readonly DbQuizPersistence dbQuizPersistence;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizManager"/> class.
        /// </summary>
        /// <param name="quizManagerEvents">Global class with quiz manager events.</param>
        /// <param name="dbQuizPersistence">Database context for quizzes representation.</param>
        public QuizManager(
            QuizManagerEvents quizManagerEvents,
            DbQuizPersistence dbQuizPersistence)
        {
            this.quizManagerEvents = quizManagerEvents ?? throw new ArgumentNullException(nameof(quizManagerEvents));
            this.dbQuizPersistence = dbQuizPersistence ?? throw new ArgumentNullException(nameof(dbQuizPersistence));
        }

        /// <summary>
        /// Start new quiz for user.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <param name="startedAt">Time when new quiz started.</param>
        /// <returns>New quiz for user.</returns>
        public Quiz StartQuiz(long chatId, DateTimeOffset startedAt)
        {
            if (this.dbQuizPersistence.IsChatExist(chatId))
            {
                this.dbQuizPersistence.RegisterNewChat(chatId);
            }

            var startedQuize = this.GetQuiz(1) ?? throw new InvalidOperationException();
            this.quizManagerEvents.NotifyQuizStarted(chatId, startedQuize);
            return startedQuize;
        }

        /// <summary>
        /// Register user answer.
        /// </summary>
        /// <param name="chatId">Chat Id.</param>
        /// <param name="userAnswerkey">User answer key.</param>
        public void RegisterUserAnswer(long chatId, string userAnswerkey)
        {
            var quiz = this.GetQuiz(1) ?? throw new InvalidOperationException();
            var isRightAnswer = quiz.IsRightAnswer(userAnswerkey);
            this.quizManagerEvents.NotifyUserAnswerResultRecived(chatId, isRightAnswer);
        }

        /// <summary>
        /// Get quiz by id.
        /// </summary>
        /// <param name="quizId">Id of the requested quiz.</param>
        /// <returns>Quiz model if it found.</returns>
        public Quiz? GetQuiz(int quizId)
        {
            return this.dbQuizPersistence.GetQuiz(quizId);
        }

        /// <summary>
        /// Get quizzes by requested parameters.
        /// </summary>
        /// <param name="fromDate">Date from which quiz was created.</param>
        /// <param name="toDate">Date before which quiz was created.</param>
        /// <param name="isActive">Indicate that quiz is active.</param>
        /// <returns>Collection of Quiz modeles from database.</returns>
        public IEnumerable<Quiz> GetQuizzes(
            DateTimeOffset fromDate,
            DateTimeOffset toDate,
            bool? isActive)
        {
            return this.dbQuizPersistence.GetQuizzes(
                fromDate,
                toDate,
                isActive);
        }
    }
}