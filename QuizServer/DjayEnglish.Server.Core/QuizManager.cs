// -----------------------------------------------------------------------
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
        /// Get quizzes without audio files.
        /// </summary>
        /// <returns>Quiz collection which does not have audio.</returns>
        public IEnumerable<Quiz> GetQuizzesWithoutAudio()
        {
            var result = this.dbQuizPersistence.GetQuizzesWithoutAudio();
            return result;
        }

        /// <summary>
        /// Set quiz audio enable.
        /// </summary>
        /// /<param name="quizId">Id of quiz for which set enable.</param>
        /// /<param name="hasAudio">Indicate that quiz has audio.</param>
        public void SetQuizAudioEnable(int quizId, bool hasAudio)
        {
            this.dbQuizPersistence.SetQuizAudioEnable(quizId, hasAudio);
        }

        /// <summary>
        /// Start new quiz for user.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <param name="startedAt">Time when new quiz started.</param>
        /// <returns>New quiz for user.</returns>
        public Quiz StartQuiz(long chatId, DateTimeOffset startedAt)
        {
            if (!this.dbQuizPersistence.IsChatExist(chatId))
            {
                this.dbQuizPersistence.RegisterNewChat(chatId);
            }

            var startedQuize = this.GetNextQuizForUser(chatId);
            this.dbQuizPersistence.AddQuizToUser(chatId, startedQuize.Id, startedAt);
            this.quizManagerEvents.NotifyQuizStarted(chatId, startedQuize);
            return startedQuize;
        }

        /// <summary>
        /// Register user answer.
        /// </summary>
        /// <param name="chatId">Chat Id.</param>
        /// <param name="userAnswerkey">User answer key.</param>
        /// <param name="answerDate">Date time when user answer on quiz.</param>
        public void RegisterUserAnswer(
            long chatId,
            string userAnswerkey,
            DateTimeOffset answerDate)
        {
            var chatQuize = this.dbQuizPersistence.GetActiveChatQuiz(chatId);
            if (chatQuize == null)
            {
                throw new InvalidOperationException(
                    $"Chat {chatId} doesn't have active quize. User answer could'nt be registered.");
            }

            var quiz = this.GetQuiz(chatQuize.QuizId);
            if (quiz == null)
            {
                throw new InvalidOperationException(
                    $"User answer couldn't be registered in chat {chatId} because quize {chatQuize.QuizId} is null.");
            }

            var answerOption = quiz.GetAnswerOption(userAnswerkey);
            if (answerOption == null)
            {
                return;
            }

            this.dbQuizPersistence.RegisterQuizAnswer(chatQuize.Id, answerOption.Id);
            this.quizManagerEvents.NotifyUserAnswerResultRecived(chatId, answerOption.IsRightAnswer);

            if (answerOption.IsRightAnswer)
            {
                this.StartQuiz(chatId, answerDate);
            }
        }

        /// <summary>
        /// Get next quiz for user.
        /// </summary>
        /// <param name="chatId">Id of the chat which identify user.</param>
        /// <returns>Quiz model for user.</returns>
        public Quiz GetNextQuizForUser(long chatId)
        {
            var newForChatQuiz = this.dbQuizPersistence.GetNewForChatActiveQuiz(chatId);
            if (newForChatQuiz != null)
            {
                return newForChatQuiz;
            }

            var oldestQuizId = this.dbQuizPersistence.GetOldestActiveQuizId(chatId);
            if (oldestQuizId != null)
            {
                var oldestQuize = this.GetQuiz(oldestQuizId.Value);
                if (oldestQuize == null)
                {
                    throw new InvalidOperationException(
                        $"Oldest quiz model with id {oldestQuizId.Value} couldn't be null for chat {chatId}");
                }
            }

            var randomQuize = this.dbQuizPersistence.GetRandomActiveQuizId();
            if (randomQuize == null)
            {
                throw new InvalidOperationException(
                        $"Random quiz couldn't be null for chat {chatId}");
            }

            return randomQuize;
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
