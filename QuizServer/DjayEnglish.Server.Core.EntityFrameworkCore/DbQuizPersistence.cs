// -----------------------------------------------------------------------
// <copyright file="DbQuizPersistence.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DjayEnglish.EntityFramework;
    using DjayEnglish.Server.ObjectModels;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Quiz persistence.
    /// </summary>
    public class DbQuizPersistence
    {
        private readonly DjayEnglishDBContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQuizPersistence"/> class.
        /// </summary>
        /// <param name="dbContext">Database context to use.</param>
        /// <param name="mapper">Map entity models to object models.</param>
        public DbQuizPersistence(
            DjayEnglishDBContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Check if chat with id exist.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <returns>Indicate that chat with requested id exist.</returns>
        public bool IsChatExist(long chatId)
        {
            var chat = this.dbContext.Chats.FirstOrDefault(_ => _.Id == chatId);
            return chat != null;
        }

        /// <summary>
        /// Register new chat.
        /// </summary>
        /// <param name="chatId">Id of the new chat.</param>
        public void RegisterNewChat(long chatId)
        {
            var newChat = new Chat()
            {
                Id = chatId,
            };

            this.dbContext.Chats.Add(newChat);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Add new quiz to user chat.
        /// </summary>
        /// <param name="chatId">Id of the chat for which quiz added.</param>
        /// <param name="quizId">Id of the quiz which will be added to chat.</param>
        /// <param name="created">Date when quiz added to chat.</param>
        /// <returns>Model of chat quiz.</returns>
        public ChatQuiz AddQuizToChat(
            long chatId,
            int quizId,
            DateTimeOffset created)
        {
            var newChatQuiz = new ChatQuiz()
            {
                ChatId = chatId,
                QuizId = quizId,
                State = (byte)ChatQuizState.IsActive,
                Created = created,
            };

            var entityChatQuiz = this.dbContext.ChatQuizzes.Add(newChatQuiz).Entity;
            this.dbContext.SaveChanges();
            return entityChatQuiz;
        }

        /// <summary>
        /// Get active chat quize.
        /// </summary>
        /// <param name="chatId">Id of the chat for which return quiz.</param>
        /// <returns>Model of chat quiz.</returns>
        public ChatQuiz? GetActiveChatQuiz(long chatId)
        {
            var entityChatQuiz = this.dbContext.ChatQuizzes
                .Where(_ => _.ChatId == chatId && _.State == (byte)ChatQuizState.IsActive)
                .OrderByDescending(_ => _.Created)
                .FirstOrDefault();
            this.dbContext.SaveChanges();
            return entityChatQuiz;
        }

        /// <summary>
        /// Register user answer on quiz.
        /// </summary>
        /// <param name="chatQuizId">Id of the chat quize where user give answer.</param>
        /// <param name="answerId">Id of the quiz answer.</param>
        public void RegisterQuizAnswer(
            int chatQuizId,
            int answerId)
        {
            var newChatQuizAnswer = new ChatQuizAnswer()
            {
                ChatQuizId = chatQuizId,
                AnswerId = answerId,
            };

            this.dbContext.ChatQuizAnswers.Add(newChatQuizAnswer);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Get quiz by id.
        /// </summary>
        /// <param name="quizId">Id of the requested quiz.</param>
        /// <returns>Quiz model if it found in database.</returns>
        public ObjectModels.Quiz? GetQuiz(int quizId)
        {
            var quizEntity = this.dbContext.Quizzes
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.WordUsage)
                .FirstOrDefault(_ => _.Id == quizId);
            var quizModel = this.mapper.Map<ObjectModels.Quiz?>(quizEntity);
            return quizModel;
        }

        /// <summary>
        /// Get quizzes by requested parameters.
        /// </summary>
        /// <param name="fromDate">Date from which quiz was created.</param>
        /// <param name="toDate">Date before which quiz was created.</param>
        /// <param name="isActive">Indicate that quiz is active.</param>
        /// <returns>Quiz model if it found in database.</returns>
        public IEnumerable<ObjectModels.Quiz> GetQuizzes(
            DateTimeOffset fromDate,
            DateTimeOffset toDate,
            bool? isActive)
        {
            var quizzesEntity = this.dbContext.Quizzes
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.WordUsage)
                .Where(_ => _.Created >= fromDate && _.Created <= toDate);
            if (isActive != null)
            {
                quizzesEntity = quizzesEntity.Where(_ => _.IsActive == isActive.Value);
            }

            var quizzesModel = this.mapper.Map<IEnumerable<ObjectModels.Quiz>>(quizzesEntity);
            return quizzesModel;
        }
    }
}
